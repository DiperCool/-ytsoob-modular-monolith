using Asp.Versioning.Conventions;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Web;
using BuildingBlocks.Core.Exception.Types;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Posts.Posts.Exception;
using Ytsoob.Modules.Posts.Posts.Models;
using Ytsoob.Modules.Posts.Shared.Contracts;
using Ytsoob.Modules.Posts.Shared.Data;

namespace Ytsoob.Modules.Posts.Contents.Features.AddingFiles.v1.AddFiles;

public record AddFiles(long PostId, IEnumerable<IFormFile> Files) : ITxUpdateCommand;

public static class AddFilesEndpoint
{
    internal static IEndpointRouteBuilder MapAddFilesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPost($"{ContentsConfig.PostsPrefixUri}/{nameof(AddFiles).Kebaberize()}", HandleAsync)
            .RequireAuthorization()
            .WithTags(ContentsConfig.Tag)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName(nameof(AddFiles))
            .WithDisplayName(nameof(AddFiles).Humanize())
            .WithApiVersionSet(ContentsConfig.VersionSet)
            .HasApiVersion(1.0);
        return endpoints;
    }

    private static async Task<IResult> HandleAsync(
        long postId,
        IFormFileCollection files,
        IGatewayProcessor<PostsModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken
    )
    {
        return await gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            await commandProcessor.SendAsync(new AddFiles(postId, files), cancellationToken);

            return Results.NoContent();
        });
    }
}

public class AddFilesHandler : ICommandHandler<AddFiles>
{
    private IContentBlobStorage _contentBlobStorage;
    private PostsDbContext _postsDbContext;
    private ICurrentUserService _currentUserService;
    private IMapper _mapper;

    public AddFilesHandler(
        IContentBlobStorage contentBlobStorage,
        PostsDbContext postsDbContext,
        IMapper mapper,
        ICurrentUserService currentUserService
    )
    {
        _contentBlobStorage = contentBlobStorage;
        _postsDbContext = postsDbContext;
        _mapper = mapper;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(AddFiles request, CancellationToken cancellationToken)
    {
        Post? post = await _postsDbContext
            .Posts.Include(x => x.Content)
            .Where(x => x.CreatedBy == _currentUserService.YtsooberId && x.Id == request.PostId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (post == null)
            throw new PostNotFoundException(request.PostId);
        if (!request.Files.Any())
            throw new BadRequestException("Files empty");
        IEnumerable<string?> files = await _contentBlobStorage.UploadFilesAsync(request.Files, cancellationToken);
        foreach (var file in files)
        {
            if (file != null)
                post.AddFileToContent(file);
        }

        _postsDbContext.Posts.Update(post);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
