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
using Ytsoob.Modules.Posts.Contents.Features.AddingFiles.v1.AddFiles;
using Ytsoob.Modules.Posts.Posts.Exception;
using Ytsoob.Modules.Posts.Posts.Models;
using Ytsoob.Modules.Posts.Shared.Contracts;
using Ytsoob.Modules.Posts.Shared.Data;

namespace Ytsoob.Modules.Posts.Contents.Features.RemovingFiles.v1.RemoveFiles;

public record RemoveFiles(long PostId, IEnumerable<string> Files) : ICommand;

public static class RemoveFilesEndpoint
{
    internal static IEndpointRouteBuilder MapRemoveFilesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapDelete($"{ContentsConfig.PostsPrefixUri}/{nameof(RemoveFiles).Kebaberize()}", HandleAsync)
            .RequireAuthorization()
            .WithTags(ContentsConfig.Tag)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName(nameof(RemoveFiles))
            .WithDisplayName(nameof(RemoveFiles).Humanize())
            .WithApiVersionSet(ContentsConfig.VersionSet)
            .HasApiVersion(1.0);
        return endpoints;
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] RemoveFiles request,
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

public class RemoveFilesHandler : ICommandHandler<RemoveFiles>
{
    private PostsDbContext _context;
    private ICurrentUserService _currentUserService;
    private IContentBlobStorage _contentBlobStorage;

    public RemoveFilesHandler(
        PostsDbContext context,
        ICurrentUserService currentUserService,
        IContentBlobStorage contentBlobStorage
    )
    {
        _context = context;
        _currentUserService = currentUserService;
        _contentBlobStorage = contentBlobStorage;
    }

    public async Task<Unit> Handle(RemoveFiles request, CancellationToken cancellationToken)
    {
        Post? post = await _context
            .Posts.Include(x => x.Content)
            .Where(x => x.CreatedBy == _currentUserService.YtsooberId && x.Id == request.PostId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (post == null)
            throw new PostNotFoundException(request.PostId);
        foreach (string file in request.Files)
        {
            post.RemoveFileFromContent(file);
        }

        await _contentBlobStorage.RemoveFilesAsync(request.Files, cancellationToken);
        return Unit.Value;
    }
}
