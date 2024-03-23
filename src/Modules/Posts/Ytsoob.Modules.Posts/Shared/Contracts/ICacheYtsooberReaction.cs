using Ytsoob.Modules.Posts.Reactions.Enums;

namespace Ytsoob.Modules.Posts.Shared.Contracts;

public interface ICacheYtsooberReaction
{
    public Task<ReactionType?> GetYtsooberReactionAsync(string entityId, long ytsooberId, string entityType);
    public Task RemoveCache(string entityId, long ytsooberId, string entityType);
}
