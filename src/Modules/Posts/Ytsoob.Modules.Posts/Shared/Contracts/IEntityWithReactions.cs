using Ytsoob.Modules.Posts.Reactions.Enums;
using Ytsoob.Modules.Posts.Reactions.Models;

namespace Ytsoob.Modules.Posts.Shared.Contracts;

public interface IEntityWithReactions<TId>
{
    public TId Id { get; }
    public ReactionStats ReactionStats { get; }

    public void AddReaction(ReactionType reactionType);
    public void RemoveReaction(ReactionType reactionType);
}
