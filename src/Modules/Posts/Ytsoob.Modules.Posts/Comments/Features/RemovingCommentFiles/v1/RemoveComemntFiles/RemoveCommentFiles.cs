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
using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Posts.Comments.Exceptions;
using Ytsoob.Modules.Posts.Comments.Models;
using Ytsoob.Modules.Posts.Shared.Contracts;
using Ytsoob.Modules.Posts.Shared.Data;

namespace Ytsoob.Modules.Posts.Comments.Features.RemovingCommentFiles.v1.RemoveComemntFiles;

public record RemoveCommentFiles(long CommentId, IEnumerable<string> Files) : ITxUpdateCommand;

public static class RemoveCommentFilesEndpoint
{
    internal static IEndpointRouteBuilder MapRemoveCommentFilesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapDelete($"{CommentsConfigs.PrefixUri}/{nameof(RemoveCommentFiles).Kebaberize()}", HandleAsync)
            .RequireAuthorization()
            .WithTags(CommentsConfigs.Tag)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName(nameof(RemoveCommentFiles))
            .WithDisplayName(nameof(RemoveCommentFiles).Humanize())
            .WithApiVersionSet(CommentsConfigs.VersionSet)
            .HasApiVersion(1.0);
        return endpoints;
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] RemoveCommentFiles request,
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

public class RemoveCommentFilesHandler : ICommandHandler<RemoveCommentFiles>
{
    private PostsDbContext _postsDbContext;
    private ICommentFilesBlobStorage _blob;
    private ICurrentUserService _currentUserService;

    public RemoveCommentFilesHandler(
        ICurrentUserService currentUserService,
        ICommentFilesBlobStorage blob,
        PostsDbContext postsDbContext
    )
    {
        _currentUserService = currentUserService;
        _blob = blob;
        _postsDbContext = postsDbContext;
    }

    public async Task<Unit> Handle(RemoveCommentFiles request, CancellationToken cancellationToken)
    {
        BaseComment? comment = await _postsDbContext.BaseComments.FirstOrDefaultAsync(
            x => x.Id == request.CommentId && x.CreatedBy == _currentUserService.YtsooberId,
            cancellationToken: cancellationToken
        );
        if (comment == null)
        {
            throw new CommentNotFoundException(request.CommentId);
        }

        foreach (var file in request.Files)
        {
            comment.RemoveFile(file);
        }

        _postsDbContext.BaseComments.Update(comment);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
