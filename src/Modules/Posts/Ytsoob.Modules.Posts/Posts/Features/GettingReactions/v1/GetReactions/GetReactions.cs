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
using Ytsoob.Modules.Posts.Posts.ValueObjects;
using Ytsoob.Modules.Posts.Reactions.Dtos;
using Ytsoob.Modules.Posts.Reactions.Models;
using Ytsoob.Modules.Posts.Shared.Data;

namespace Ytsoob.Modules.Posts.Posts.Features.GettingReactions.v1.GetReactions;

public record GetReactionsResponse(ListResultModel<ReactionDto> Reactions);

public record GetReactions(long PostId) : ListQuery<GetReactionsResponse>;

public class GetPostsValidator : AbstractValidator<GetReactions>
{
    public GetPostsValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Page).GreaterThanOrEqualTo(1).WithMessage("Page should at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageSize should at least greater than or equal to 1.");
    }
}

public static class GetReactionsEndpoint
{
    internal static IEndpointRouteBuilder MapGetReactionsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGet($"{PostsConfigs.PostsPrefixUri}/{nameof(GetReactions).Kebaberize()}", HandleAsync)
            .RequireAuthorization()
            .WithTags(PostsConfigs.Tag)
            .Produces<GetReactionsResponse>()
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName(nameof(GetReactions))
            .WithDisplayName(nameof(GetReactions).Humanize())
            .WithApiVersionSet(PostsConfigs.VersionSet)
            .HasApiVersion(1.0);
        return endpoints;
    }

    private static async Task<IResult> HandleAsync(
        long postId,
        IGatewayProcessor<PostsModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken,
        int page = 1,
        int pageSize = 15
    )
    {
        return await gatewayProcessor.ExecuteQuery(async commandProcessor =>
        {
            await commandProcessor.SendAsync(
                new GetReactions(postId) { Page = page, PageSize = pageSize },
                cancellationToken
            );

            return Results.NoContent();
        });
    }
}

public class GetReactionsHandler : IQueryHandler<GetReactions, GetReactionsResponse>
{
    private PostsDbContext _postsDbContext;
    private IMapper _mapper;
    private ICurrentUserService _currentUserService;

    public GetReactionsHandler(PostsDbContext postsDbContext, IMapper mapper, ICurrentUserService currentUserService)
    {
        _postsDbContext = postsDbContext;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<GetReactionsResponse> Handle(GetReactions request, CancellationToken cancellationToken)
    {
        PostId postId = PostId.Of(request.PostId);
        var reactions = await _postsDbContext
            .Reactions.Where(x => x.EntityId == postId.ToString())
            .Include(x => x.Ytsoober)
            .AsNoTracking()
            .ApplyPagingAsync<Reaction, ReactionDto>(
                _mapper.ConfigurationProvider,
                request.Page,
                request.PageSize,
                cancellationToken: cancellationToken
            );

        return new GetReactionsResponse(reactions);
    }
}
