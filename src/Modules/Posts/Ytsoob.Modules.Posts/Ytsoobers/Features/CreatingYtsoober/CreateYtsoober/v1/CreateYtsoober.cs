using BuildingBlocks.Abstractions.CQRS.Command;
using MediatR;
using Microsoft.Extensions.Logging;
using Ytsoob.Modules.Posts.Shared.Data;
using Ytsoob.Modules.Posts.Ytsoobers.Models;

namespace Ytsoob.Modules.Posts.Ytsoobers.Features.CreatingYtsoober.CreateYtsoober.v1;

public record CreateYtsoober(long Id, Guid IdentityId, string? Username, string Email, string Avatar) : ITxCommand;

internal class CreateYtsooberHandler : ICommandHandler<CreateYtsoober>
{
    private PostsDbContext _postsDbContext;
    private ILogger<CreateYtsooberHandler> _logger;

    public CreateYtsooberHandler(PostsDbContext postsDbContext, ILogger<CreateYtsooberHandler> logger)
    {
        _postsDbContext = postsDbContext;
        _logger = logger;
    }

    public async Task<Unit> Handle(CreateYtsoober request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating ytsoober");
        Ytsoober ytsoober = new Ytsoober()
        {
            Id = request.Id,
            Email = request.Email,
            Username = request.Username,
            IdentityId = request.IdentityId,
            Avatar = request.Avatar
        };
        await _postsDbContext.Ytsoobers.AddAsync(ytsoober, cancellationToken);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Ytsoober created with ID = {ID}", ytsoober.Id);
        return Unit.Value;
    }
}
