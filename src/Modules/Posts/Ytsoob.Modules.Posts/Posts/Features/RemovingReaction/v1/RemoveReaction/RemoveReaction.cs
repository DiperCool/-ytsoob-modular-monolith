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
using Ytsoob.Modules.Posts.Posts.Features.GettingReactions.v1.GetReactions;
using Ytsoob.Modules.Posts.Posts.Models;
using Ytsoob.Modules.Posts.Posts.ValueObjects;
using Ytsoob.Modules.Posts.Shared.Contracts;

namespace Ytsoob.Modules.Posts.Posts.Features.RemovingReaction.v1.RemoveReaction;

public record RemoveReaction(long Id) : ITxUpdateCommand;

public static class RemoveReactionEndpoint
{
    internal static IEndpointRouteBuilder MapRemoveReactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapDelete($"{PostsConfigs.PostsPrefixUri}/{nameof(RemoveReaction).Kebaberize()}", HandleAsync)
            .RequireAuthorization()
            .WithTags(PostsConfigs.Tag)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName(nameof(RemoveReaction))
            .WithDisplayName(nameof(RemoveReaction).Humanize())
            .WithApiVersionSet(PostsConfigs.VersionSet)
            .HasApiVersion(1.0);
        return endpoints;
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] RemoveReaction request,
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

public class RemoveReactionHandler : ICommandHandler<RemoveReaction>
{
    private IReactionService _reactionService;
    private ICurrentUserService _currentUserService;
    private ICacheYtsooberReaction _ytsooberReactionCache;

    public RemoveReactionHandler(
        IReactionService reactionService,
        ICurrentUserService currentUserService,
        ICacheYtsooberReaction ytsooberReactionCache
    )
    {
        _reactionService = reactionService;
        _currentUserService = currentUserService;
        _ytsooberReactionCache = ytsooberReactionCache;
    }

    public async Task<Unit> Handle(RemoveReaction request, CancellationToken cancellationToken)
    {
        PostId postId = PostId.Of(request.Id);
        await _reactionService.RemoveReactionAsync<Post, PostId>(
            PostId.Of(request.Id),
            _currentUserService.YtsooberId,
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
