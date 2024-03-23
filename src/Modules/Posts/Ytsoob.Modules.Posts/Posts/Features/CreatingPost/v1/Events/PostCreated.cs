using BuildingBlocks.Abstractions.CQRS.Event.Internal;
using BuildingBlocks.Core.CQRS.Event.Internal;
using Microsoft.Extensions.Logging;
using Ytsoob.Modules.Posts.Posts.Models;

namespace Ytsoob.Modules.Posts.Posts.Features.CreatingPost.v1.Events;

public record PostCreated(Post Post) : DomainEvent;

public class PostCreatedHandler : IDomainEventHandler<PostCreated>
{
    private ILogger<PostCreatedHandler> _logger;

    public PostCreatedHandler(ILogger<PostCreatedHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(PostCreated notification, CancellationToken cancellationToken) { }
}
