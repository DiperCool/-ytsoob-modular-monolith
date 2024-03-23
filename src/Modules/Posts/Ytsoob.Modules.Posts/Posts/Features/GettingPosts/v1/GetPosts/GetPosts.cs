using Asp.Versioning.Conventions;
using AutoMapper;
using BuildingBlocks.Abstractions.Web;
using BuildingBlocks.Core.CQRS.Query;
using BuildingBlocks.Core.Persistence.EfCore;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Posts.Polls.Dtos;
using Ytsoob.Modules.Posts.Posts.Dtos;
using Ytsoob.Modules.Posts.Posts.Models;
using Ytsoob.Modules.Posts.Posts.ValueObjects;
using Ytsoob.Modules.Posts.Shared.Contracts;
using Ytsoob.Modules.Posts.Shared.Data;

namespace Ytsoob.Modules.Posts.Posts.Features.GettingPosts.v1.GetPosts;

public record GetPostsResponse(ListResultModel<PostDto> Posts);

public record GetPosts(long YtsooberId) : ListQuery<GetPostsResponse>;

public static class GetPostsEndpoint
{
    internal static IEndpointRouteBuilder MapGetPostsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGet($"{PostsConfigs.PostsPrefixUri}/{nameof(GetPosts).Kebaberize()}", HandleAsync)
            .AllowAnonymous()
            .WithTags(PostsConfigs.Tag)
            .Produces<GetPostsResponse>()
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName(nameof(GetPosts))
            .WithDisplayName(nameof(GetPosts).Humanize())
            .WithApiVersionSet(PostsConfigs.VersionSet)
            .HasApiVersion(1.0);
        return endpoints;
    }

    private static async Task<IResult> HandleAsync(
        long ytsooberId,
        IGatewayProcessor<PostsModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken,
        int page = 1,
        int pageSize = 15
    )
    {
        return await gatewayProcessor.ExecuteQuery(async commandProcessor =>
        {
            var res = await commandProcessor.SendAsync(
                new GetPosts(ytsooberId) { Page = page, PageSize = pageSize },
                cancellationToken
            );

            return Results.Ok(res);
        });
    }
}

public class GetPostsHandler : IRequestHandler<GetPosts, GetPostsResponse>
{
    private PostsDbContext _postsDbContext;
    private IMapper _mapper;
    private ICacheYtsooberOptions _cacheYtsooberOptions;
    private ICacheYtsooberReaction _cacheYtsooberReaction;
    private ICurrentUserService _currentUserService;

    public GetPostsHandler(
        PostsDbContext postsDbContext,
        IMapper mapper,
        ICacheYtsooberOptions cacheYtsooberOptions,
        ICurrentUserService currentUserService,
        ICacheYtsooberReaction cacheYtsooberReaction
    )
    {
        _postsDbContext = postsDbContext;
        _mapper = mapper;
        _cacheYtsooberOptions = cacheYtsooberOptions;
        _currentUserService = currentUserService;
        _cacheYtsooberReaction = cacheYtsooberReaction;
    }

    public async Task<GetPostsResponse> Handle(GetPosts request, CancellationToken cancellationToken)
    {
        var posts = await _postsDbContext
            .Posts.Include(x => x.Poll)
            .ThenInclude(x => x!.Options)
            .Include(x => x.Content)
            .Include(x => x.ReactionStats)
            .Where(x => x.CreatedBy == request.YtsooberId)
            .OrderByDescending(x => x.Created)
            .AsNoTracking()
            .ApplyPagingAsync<Post, PostDto>(
                _mapper.ConfigurationProvider,
                request.Page,
                request.PageSize,
                cancellationToken: cancellationToken
            );
        if (_currentUserService.IsAuthenticated)
        {
            await CalculateUserVotes(posts);
            await CalculateUserReaction(posts);
        }

        return new GetPostsResponse(posts);
    }

    private async Task CalculateUserVotes(ListResultModel<PostDto> posts)
    {
        foreach (PollDto? poll in posts.Items.Select(x => x.Poll))
        {
            if (poll == null)
                continue;
            poll.UserVotedOption = await _cacheYtsooberOptions.GetUsersOptionsInPollAsync(
                poll.Id,
                _currentUserService.YtsooberId
            );
        }
    }

    public async Task CalculateUserReaction(ListResultModel<PostDto> posts)
    {
        foreach (PostDto post in posts.Items)
        {
            post.YtsooberReaction = await _cacheYtsooberReaction.GetYtsooberReactionAsync(
                PostId.Of(post.Id).ToString(),
                _currentUserService.YtsooberId,
                typeof(Post).ToString()
            );
        }
    }
}
