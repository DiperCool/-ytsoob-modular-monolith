using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Web;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Ytsoob.Modules.Posts.Posts.Models;
using Ytsoob.Modules.Posts.Posts.ValueObjects;
using Ytsoob.Modules.Posts.Reactions.Enums;
using Ytsoob.Modules.Posts.Shared.Contracts;

namespace Ytsoob.Modules.Posts.Posts.Features.AddingReaction.v1.AddReaction;

public record AddReaction(long Id, ReactionType ReactionType) : ITxCreateCommand;

public static class AddReactEndpoint
{
    internal static IEndpointRouteBuilder MapAddReactEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPost($"{PostsConfigs.PostsPrefixUri}/{nameof(AddReaction).Kebaberize()}", HandleAsync)
            .RequireAuthorization()
            .WithTags(PostsConfigs.Tag)
            .Produces(StatusCodes.Status201Created)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName(nameof(AddReaction))
            .WithDisplayName(nameof(AddReaction).Humanize())
            .WithApiVersionSet(PostsConfigs.VersionSet)
            .HasApiVersion(1.0);
        return endpoints;
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] AddReaction request,
        IGatewayProcessor<PostsModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken
    )
    {
        return await gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            await commandProcessor.SendAsync(request, cancellationToken);

            return Results.NoContent();
        });
    }
}

public class AddReactionHandler : ICommandHandler<AddReaction>
{
    private IReactionService _reactionService;
    private ICurrentUserService _currentUserService;
    private ICacheYtsooberReaction _ytsooberReactionCache;

    public AddReactionHandler(
        IReactionService reactionService,
        ICurrentUserService currentUserService,
        ICacheYtsooberReaction ytsooberReactionCache
    )
    {
        _reactionService = reactionService;
        _currentUserService = currentUserService;
        _ytsooberReactionCache = ytsooberReactionCache;
    }

    public async Task<Unit> Handle(AddReaction request, CancellationToken cancellationToken)
    {
        PostId postId = PostId.Of(request.Id);
        await _reactionService.AddReactionAsync<Post, PostId>(
            postId,
            _currentUserService.YtsooberId,
            request.ReactionType,
            cancellationToken
        );
        await _ytsooberReactionCache.RemoveCache(
            postId.ToString(),
            _currentUserService.YtsooberId,
            typeof(Post).ToString()
        );
        return Unit.Value;
    }
}
