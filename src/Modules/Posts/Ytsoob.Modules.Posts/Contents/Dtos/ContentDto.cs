namespace Ytsoob.Modules.Posts.Contents.Dtos;

public class ContentDto
{
    public string ContentText { get; set; } = default!;
    public IEnumerable<string> Files { get; set; } = null!;
}
