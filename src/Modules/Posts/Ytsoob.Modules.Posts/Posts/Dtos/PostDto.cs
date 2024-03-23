using Ytsoob.Modules.Posts.Contents.Dtos;
using Ytsoob.Modules.Posts.Polls.Dtos;
using Ytsoob.Modules.Posts.Reactions.Dtos;
using Ytsoob.Modules.Posts.Reactions.Enums;

namespace Ytsoob.Modules.Posts.Posts.Dtos;

public class PostDto
{
    public long Id { get; set; }
    public ContentDto? Content { get; set; }
    public PollDto? Poll { get; set; }
    public ReactionStatsDto ReactionStats { get; set; } = default!;
    public ReactionType? YtsooberReaction { get; set; }
}
