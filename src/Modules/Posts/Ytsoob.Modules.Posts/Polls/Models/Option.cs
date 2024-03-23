using BuildingBlocks.Core.Domain;
using Ytsoob.Modules.Posts.Polls.ValueObjects;
using Ytsoob.Modules.Posts.Ytsoobers.Models;

namespace Ytsoob.Modules.Posts.Polls.Models;

public class Option : Entity<OptionId>
{
    public OptionTitle Title { get; private set; } = null!;

    public OptionCount Count { get; private set; } = null!;
    public Fiction Fiction { get; private set; } = null!;
    public PollId PollId { get; private set; }
    public Poll Poll { get; set; }

    // ef core
    protected Option() { }

    protected Option(OptionId id, OptionTitle title)
    {
        Id = id;
        Title = title;
        Count = OptionCount.Of(0);
        Fiction = Fiction.Of(0);
    }

    public static Option Create(OptionId optionId, OptionTitle title)
    {
        Option option = new Option(optionId, title);
        return option;
    }

    public void Vote(Ytsoober voter)
    {
        Count = OptionCount.Of(Count.Value + 1);
    }

    public void RecalculateFiction(TotalCountPoll totalCountPoll)
    {
        Fiction = Fiction.Of(totalCountPoll == 0 ? 0 : ((decimal)Count / totalCountPoll) * 100);
    }

    public void Unvote(Ytsoober voter)
    {
        Count = OptionCount.Of(Count.Value - 1);
    }
}
