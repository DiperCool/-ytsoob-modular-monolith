using Ytsoob.Modules.Posts.Polls.Models;
using Ytsoob.Modules.Posts.Polls.ValueObjects;

namespace Ytsoob.Modules.Posts.Shared.Contracts;

public interface IPollStrategy
{
    public string PollType { get; }
    public Task Vote(Poll poll, OptionId optionId, long voterId);
    public Task Unvote(Poll poll, OptionId optionId, long voterId);
    public bool Check(string pollType)
    {
        return pollType == PollType;
    }
}
