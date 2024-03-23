namespace Ytsoob.Modules.Posts.Contents.Features.UpdatingPostContent.v1;

public class UpdatePostContentRequest
{
    public long PostId { get; set; }
    public string ContentText { get; set; } = string.Empty;
}
