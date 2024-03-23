using Asp.Versioning.Conventions;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Query;
using BuildingBlocks.Abstractions.Web;
using BuildingBlocks.Core.CQRS.Query;
using BuildingBlocks.Core.Persistence.EfCore;
using BuildingBlocks.Security.Jwt;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Humanizer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Posts.Comments.Dtos;
using Ytsoob.Modules.Posts.Comments.Models;
using Ytsoob.Modules.Posts.Comments.ValueObjects;
using Ytsoob.Modules.Posts.Shared.Contracts;
using Ytsoob.Modules.Posts.Shared.Data;

namespace Ytsoob.Modules.Posts.Comments.Features.GettingRepliedComments.v1.GetRepliedComments;

public record GetRepliedCommentsResponse(ListResultModel<CommentDto> Result);

public record GetRepliedComments(long PostId) : ListQuery<GetRepliedCommentsResponse>;

public static class GetRepliedCommentsEndpoint
{
    internal static IEndpointRouteBuilder MapGetRepliedComments(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGet($"{CommentsConfigs.PrefixUri}/{nameof(GetRepliedComments).Kebaberize()}", HandleAsync)
            .AllowAnonymous()
            .WithTags(CommentsConfigs.Tag)
            .Produces<GetRepliedCommentsResponse>()
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName(nameof(GetRepliedComments))
            .WithDisplayName(nameof(GetRepliedComments).Humanize())
            .WithApiVersionSet(CommentsConfigs.VersionSet)
            .HasApiVersion(1.0);
        return endpoints;
    }

    private static async Task<IResult> HandleAsync(
        long postId,
        IGatewayProcessor<PostsModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken,
        int page = 1,
        int pageSize = 20
    )
    {
        return await gatewayProcessor.ExecuteQuery(async commandProcessor =>
        {
            await commandProcessor.SendAsync(
                new GetRepliedComments(postId) { Page = page, PageSize = pageSize },
                cancellationToken
            );

            return Results.NoContent();
        });
    }
}

public class GetRepliedCommentsValidator : AbstractValidator<GetRepliedComments>
{
    public GetRepliedCommentsValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Page).GreaterThanOrEqualTo(1).WithMessage("Page should at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageSize should at least greater than or equal to 1.");
    }
}

public class GetRepliedCommentsHandler : IQueryHandler<GetRepliedComments, GetRepliedCommentsResponse>
{
    private PostsDbContext _postsDbContext;
    private IMapper _mapper;
    private ICurrentUserService _currentUserService;
    private ICacheYtsooberReaction _cacheYtsooberReaction;

    public GetRepliedCommentsHandler(
        ICurrentUserService currentUserService,
        IMapper mapper,
        PostsDbContext postsDbContext,
        ICacheYtsooberReaction cacheYtsooberReaction
    )
    {
        _currentUserService = currentUserService;
        _mapper = mapper;
        _postsDbContext = postsDbContext;
        _cacheYtsooberReaction = cacheYtsooberReaction;
    }

    public async Task<GetRepliedCommentsResponse> Handle(
        GetRepliedComments request,
        CancellationToken cancellationToken
    )
    {
        var comments = await _postsDbContext
            .RepliedComments.Include(x => x.ReactionStats)
            .Where(x => x.CommentId == request.PostId)
            .OrderBy(x => x.Created)
            .AsNoTracking()
            .ApplyPagingAsync<RepliedComment, CommentDto>(
                _mapper.ConfigurationProvider,
                request.Page,
                request.PageSize,
                cancellationToken: cancellationToken
            );
        if (_currentUserService.IsAuthenticated)
        {
            await CalculateUserReaction(comments);
        }

        return new GetRepliedCommentsResponse(comments);
    }

    private async Task CalculateUserReaction(ListResultModel<CommentDto> comments)
    {
        foreach (CommentDto comment in comments.Items)
        {
            comment.YtsooberReaction = await _cacheYtsooberReaction.GetYtsooberReactionAsync(
                CommentId.Of(comment.Id).ToString(),
                _currentUserService.YtsooberId,
                typeof(BaseComment).ToString()
            );
        }
    }
}
