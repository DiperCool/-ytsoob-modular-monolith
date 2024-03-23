using Ytsoob.Modules.Posts.Reactions.Enums;
using Ytsoob.Modules.Posts.Ytsoobers.Dtos;

namespace Ytsoob.Modules.Posts.Reactions.Dtos;

public class ReactionDto
{
    public ReactionType ReactionType { get; set; }
    public YtsooberDto Ytsoober { get; set; } = default!;
}
