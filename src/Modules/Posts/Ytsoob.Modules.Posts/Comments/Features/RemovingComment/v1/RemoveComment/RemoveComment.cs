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
using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Posts.Comments.Exceptions;
using Ytsoob.Modules.Posts.Comments.Models;
using Ytsoob.Modules.Posts.Shared.Data;

namespace Ytsoob.Modules.Posts.Comments.Features.RemovingComment.v1.RemoveComment;

public record RemoveComment(long CommentId) : ITxCommand;

public static class RemoveCommentEndpoint
{
    internal static IEndpointRouteBuilder MapRemoveCommentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapDelete($"{CommentsConfigs.PrefixUri}/{nameof(RemoveComment).Kebaberize()}", HandleAsync)
            .RequireAuthorization()
            .WithTags(CommentsConfigs.Tag)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName(nameof(RemoveComment))
            .WithDisplayName(nameof(RemoveComment).Humanize())
            .WithApiVersionSet(CommentsConfigs.VersionSet)
            .HasApiVersion(1.0);
        return endpoints;
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] RemoveComment request,
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

public class RemoveCommentHandler : ICommandHandler<RemoveComment>
{
    private PostsDbContext _postsDbContext;
    private ICurrentUserService _currentUserService;

    public RemoveCommentHandler(PostsDbContext postsDbContext, ICurrentUserService currentUserService)
    {
        _postsDbContext = postsDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(RemoveComment request, CancellationToken cancellationToken)
    {
        BaseComment? baseComment = await _postsDbContext.BaseComments.FirstOrDefaultAsync(
            x => x.CreatedBy == _currentUserService.YtsooberId,
            cancellationToken: cancellationToken
        );
        if (baseComment == null)
        {
            throw new CommentNotFoundException(request.CommentId);
        }

        _postsDbContext.BaseComments.Remove(baseComment);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
