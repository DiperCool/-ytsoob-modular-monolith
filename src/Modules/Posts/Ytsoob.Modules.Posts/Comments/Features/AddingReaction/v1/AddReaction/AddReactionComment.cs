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

namespace Ytsoob.Modules.Posts.Comments.Features.AddingReaction.v1.AddReaction;

public record AddReactionComment(long CommentId, ReactionType ReactionType) : ITxUpdateCommand;

public static class AddReactionCommentEndpoint
{
    internal static IEndpointRouteBuilder MapAddReactionCommentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPost($"{CommentsConfigs.PrefixUri}/{nameof(AddReactionComment).Kebaberize()}", HandleAsync)
            .AllowAnonymous()
            .WithTags(CommentsConfigs.Tag)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName(nameof(AddReactionComment))
            .WithDisplayName(nameof(AddReactionComment).Humanize())
            .WithApiVersionSet(CommentsConfigs.VersionSet)
            .HasApiVersion(1.0);
        return endpoints;
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] AddReactionComment request,
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

public class AddReactionHandler : ICommandHandler<AddReactionComment>
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

    public async Task<Unit> Handle(AddReactionComment request, CancellationToken cancellationToken)
    {
        CommentId commentId = CommentId.Of(request.CommentId);
        await _reactionService.AddReactionAsync<BaseComment, CommentId>(
            commentId,
            _currentUserService.YtsooberId,
            request.ReactionType,
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
