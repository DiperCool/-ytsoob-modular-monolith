using Ytsoob.Modules.Posts.Posts.Features.CreatingPost.v1.Request;

namespace Ytsoob.Modules.Posts.Posts.Features.CreatingPost.v1;

public class CreatePostRequest
{
    public ContentRequest Content { get; set; } = default!;
    public PollRequest? Poll { get; set; }
}
