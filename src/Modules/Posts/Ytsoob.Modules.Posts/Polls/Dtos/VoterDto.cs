namespace Ytsoob.Modules.Posts.Polls.Dtos;

public class VoterDto
{
    public long Id { get; set; }
    public string Username { get; set; } = default!;
    public string? Avatar { get; set; }
}
