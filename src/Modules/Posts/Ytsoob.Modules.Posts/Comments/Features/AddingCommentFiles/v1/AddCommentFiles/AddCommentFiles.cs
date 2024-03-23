using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Web;
using BuildingBlocks.Security.Jwt;
using FluentValidation;
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

namespace Ytsoob.Modules.Posts.Comments.Features.AddingCommentFiles.v1.AddCommentFiles;

public record AddCommentFiles(long CommentId, IEnumerable<IFormFile> Files) : ITxUpdateCommand;

public class AddCommentFilesValidator : AbstractValidator<AddCommentFiles>
{
    public AddCommentFilesValidator()
    {
        RuleFor(x => x.Files).Must(x => x.Count() < 10).WithMessage("Exceed limit of files");
    }
}

public static class AddCommentFilesEndpoint
{
    internal static IEndpointRouteBuilder MapAddCommentFiles(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPost($"{CommentsConfigs.PrefixUri}/{nameof(AddCommentFiles).Kebaberize()}", HandleAsync)
            .AllowAnonymous()
            .WithTags(CommentsConfigs.Tag)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName(nameof(AddCommentFiles))
            .WithDisplayName(nameof(AddCommentFiles).Humanize())
            .WithApiVersionSet(CommentsConfigs.VersionSet)
            .HasApiVersion(1.0);
        return endpoints;
    }

    private static async Task<IResult> HandleAsync(
        long postId,
        [FromForm] IFormFileCollection files,
        IGatewayProcessor<PostsModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken
    )
    {
        return await gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            await commandProcessor.SendAsync(new AddCommentFiles(postId, files), cancellationToken);

            return Results.NoContent();
        });
    }
}

public class AddCommentFilesHandler : ICommandHandler<AddCommentFiles>
{
    private ICommentFilesBlobStorage _blob;
    private ICurrentUserService _currentUserService;
    private PostsDbContext _postsDbContext;

    public AddCommentFilesHandler(
        PostsDbContext postsDbContext,
        ICurrentUserService currentUserService,
        ICommentFilesBlobStorage blob
    )
    {
        _postsDbContext = postsDbContext;
        _currentUserService = currentUserService;
        _blob = blob;
    }

    public async Task<Unit> Handle(AddCommentFiles request, CancellationToken cancellationToken)
    {
        BaseComment? comment = await _postsDbContext.BaseComments.FirstOrDefaultAsync(
            x => x.Id == request.CommentId && x.CreatedBy == _currentUserService.YtsooberId,
            cancellationToken: cancellationToken
        );
        if (comment == null)
        {
            throw new CommentNotFoundException(request.CommentId);
        }

        IEnumerable<string?> files = await _blob.UploadFilesAsync(request.Files, cancellationToken);
        foreach (var file in files)
        {
            if (file != null)
                comment.AddFile(file);
        }

        _postsDbContext.BaseComments.Update(comment);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
