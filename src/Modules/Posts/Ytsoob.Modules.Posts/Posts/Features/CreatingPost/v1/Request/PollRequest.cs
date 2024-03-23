namespace Ytsoob.Modules.Posts.Posts.Features.CreatingPost.v1.Request;

public record PollRequest(string PollType, IEnumerable<string> Options, string Question);
