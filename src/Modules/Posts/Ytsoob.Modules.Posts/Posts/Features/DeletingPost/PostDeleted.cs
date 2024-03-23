using BuildingBlocks.Core.CQRS.Event.Internal;
using Ytsoob.Modules.Posts.Posts.Models;

namespace Ytsoob.Modules.Posts.Posts.Features.DeletingPost;

public record PostDeleted(Post Post) : DomainEvent;
