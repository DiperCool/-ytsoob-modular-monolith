namespace Ytsoob.Modules.Posts.Polls.Dtos;

public class OptionDto
{
    public long Id { get; set; }
    public string Title { get; set; } = default!;
    public long Count { get; set; }
    public decimal Fiction { get; set; }
}
