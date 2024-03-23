using Ardalis.GuardClauses;
using Asp.Versioning.Conventions;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Web;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Ytsoob.Modules.Posts.Comments.Models;
using Ytsoob.Modules.Posts.Comments.ValueObjects;
using Ytsoob.Modules.Posts.Reactions.Enums;
using Ytsoob.Modules.Posts.Shared.Contracts;

namespace Ytsoob.Modules.Posts.Comments.Features.RemovingReaction.v1.RemoveReaction;

public record RemoveReactionComment(long CommentId, ReactionType ReactionType) : ITxUpdateCommand;

public static class RemoveReactionCommentEndpoint
{
    internal static IEndpointRouteBuilder MapRemoveReactionCommentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapDelete($"{CommentsConfigs.PrefixUri}/{nameof(RemoveReactionComment).Kebaberize()}", HandleAsync)
            .RequireAuthorization()
            .WithTags(CommentsConfigs.Tag)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName(nameof(RemoveReactionComment))
            .WithDisplayName(nameof(RemoveReactionComment).Humanize())
            .WithApiVersionSet(CommentsConfigs.VersionSet)
            .HasApiVersion(1.0);
        return endpoints;
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] RemoveReactionComment request,
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

public class RemoveReactionHandler : ICommandHandler<RemoveReactionComment>
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

    public async Task<Unit> Handle(RemoveReactionComment request, CancellationToken cancellationToken)
    {
        CommentId commentId = CommentId.Of(request.CommentId);
        await _reactionService.RemoveReactionAsync<BaseComment, CommentId>(
            CommentId.Of(request.CommentId),
            _currentUserService.YtsooberId,
            cancellationToken
        );
        await _ytsooberReactionCache.RemoveCache(
            commentId.ToString(),
            _currentUserService.YtsooberId,
            typeof(BaseComment).ToString()
        );
        return Unit.Value;
    }
}
