using BuildingBlocks.Core.CQRS.Event.Internal;
using Ytsoob.Modules.Posts.Contents.ValueObjects;
using Ytsoob.Modules.Posts.Posts.ValueObjects;

namespace Ytsoob.Modules.Posts.Contents.Features.UpdatingPostContent.v1.Events;

public record PostContentUpdated(PostId PostId, ContentText ContentText, IEnumerable<string> Files) : DomainEvent;
