using Ytsoob.Modules.Posts.Comments.ValueObjects;
using Ytsoob.Modules.Posts.Posts.ValueObjects;
using Ytsoob.Modules.Posts.Reactions.Models;

namespace Ytsoob.Modules.Posts.Comments.Models;

public class RepliedComment : BaseComment
{
    public CommentId CommentId { get; set; } = default!;
    public Comment Comment { get; set; } = default!;

    public BaseComment RepliedToComment { get; set; }
    public CommentId RepliedToCommentId { get; set; }

    // ef core
    protected RepliedComment() { }

    protected RepliedComment(
        CommentId commentId,
        CommentId replyTo,
        PostId postId,
        CommentId repliedToCommentId,
        CommentContent content,
        ReactionStats reactionStats
    )
        : base(commentId, postId, content, reactionStats)
    {
        CommentId = replyTo;
        RepliedToCommentId = repliedToCommentId;
    }

    public static RepliedComment Create(
        CommentId commentId,
        CommentId replyTo,
        PostId postId,
        CommentId repliedToCommentId,
        CommentContent commentContent,
        ReactionStats reactionStats
    )
    {
        return new RepliedComment(commentId, replyTo, postId, repliedToCommentId, commentContent, reactionStats);
    }
}
