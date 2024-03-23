namespace Ytsoob.Modules.Posts.Reactions.Dtos;

public class ReactionStatsDto
{
    public long Like { get; private set; } = default!;
    public long Dislike { get; private set; } = default!;
    public long Angry { get; private set; } = default!;
    public long Happy { get; private set; } = default!;
    public long Wonder { get; private set; } = default!;
    public long Crying { get; private set; } = default!;
}
