using BuildingBlocks.Core.Domain;
using Ytsoob.Modules.Posts.Reactions.Enums;
using Ytsoob.Modules.Posts.Reactions.ValueObjects;
using Ytsoob.Modules.Posts.Shared.Contracts;
using Ytsoob.Modules.Posts.Ytsoobers.Models;

namespace Ytsoob.Modules.Posts.Reactions.Models;

public class Reaction : Aggregate<ReactionId>
{
    public ReactionType ReactionType { get; private set; }
    public string EntityId { get; private set; }
    public string EntityType { get; private set; } = default!;

    public long YtsooberId { get; private set; }
    public Ytsoober Ytsoober { get; private set; }

    // ef core
    protected Reaction() { }

    protected Reaction(
        ReactionId reactionId,
        string entityId,
        string entityType,
        long ytsooberId,
        ReactionType reactionType
    )
    {
        Id = reactionId;
        EntityId = entityId;
        YtsooberId = ytsooberId;
        ReactionType = reactionType;
        EntityType = entityType;
    }

    public static Reaction Create<T, TId>(
        ReactionId reactionId,
        TId entityId,
        string entityType,
        long ytsooberId,
        ReactionType reactionType
    )
        where T : class, IEntityWithReactions<TId>
    {
        Reaction reaction = new Reaction(reactionId, entityId!.ToString()!, entityType, ytsooberId, reactionType);
        return reaction;
    }

    public void Remove() { }
}
