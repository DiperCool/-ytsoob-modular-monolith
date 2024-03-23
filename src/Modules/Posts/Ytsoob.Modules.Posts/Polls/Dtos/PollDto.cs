namespace Ytsoob.Modules.Posts.Polls.Dtos;

public class PollDto
{
    public long Id { get; set; }
    public string PollAnswerType { get; set; } = null!;
    public IList<OptionDto> Options { get; set; } = new List<OptionDto>();
    public string Question { get; set; } = default!;
    public IEnumerable<long> UserVotedOption { get; set; } = new List<long>();
}
