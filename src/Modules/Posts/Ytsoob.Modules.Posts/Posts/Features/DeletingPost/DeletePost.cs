using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Core.Exception;
using BuildingBlocks.Security.Jwt;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Posts.Posts.Exception;
using Ytsoob.Modules.Posts.Posts.ValueObjects;
using Ytsoob.Modules.Posts.Shared.Data;

namespace Ytsoob.Modules.Posts.Posts.Features.DeletingPost;

public record DeletePost(PostId PostId) : ITxCommand<Unit>;

public class DeletePostHandler : ICommandHandler<DeletePost, Unit>
{
    private PostsDbContext _postsDbContext;
    private ICurrentUserService _currentUserService;

    public DeletePostHandler(PostsDbContext postsDbContext, ICurrentUserService currentUserService)
    {
        _postsDbContext = postsDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(DeletePost request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(request));
        var post = await _postsDbContext
            .Posts.Include(x => x.Content)
            .FirstOrDefaultAsync(
                x => x.Id == request.PostId && x.CreatedBy == _currentUserService.YtsooberId,
                cancellationToken: cancellationToken
            );
        Guard.Against.NotFound(post, new PostNotFoundException(request.PostId));
        post!.Delete();
        _postsDbContext.Posts.Remove(post);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
