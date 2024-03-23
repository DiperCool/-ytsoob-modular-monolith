using BuildingBlocks.Core.Domain;
using BuildingBlocks.Core.IdsGenerator;
using Ytsoob.Modules.Posts.Polls.ValueObjects;
using Ytsoob.Modules.Posts.Posts.Models;
using Ytsoob.Modules.Posts.Posts.ValueObjects;
using Ytsoob.Modules.Posts.Ytsoobers.Models;

namespace Ytsoob.Modules.Posts.Polls.Models;

public class Poll : Entity<PollId>
{
    private List<Option> _options = new();
    public IReadOnlyCollection<Option> Options => _options.ToList();
    public PostId PostId { get; private set; }
    public Post Post { get; private set; }
    public Question Question { get; private set; }
    public TotalCountPoll TotalCountPoll { get; private set; }
    public string PollAnswerType { get; private set; }

    // ef core
    protected Poll() { }

    protected Poll(PollId id, Question question, IEnumerable<Option> options, string pollAnswerType)
    {
        Id = id;
        Question = question;
        _options = options.ToList();
        PollAnswerType = pollAnswerType;
        TotalCountPoll = TotalCountPoll.Of(0);
    }

    public static Poll Create(PollId id, Question question, IEnumerable<string> options, string pollAnswerType)
    {
        IEnumerable<Option> optionsMap = options.Select(x =>
            Option.Create(OptionId.Of(SnowFlakIdGenerator.NewId()), OptionTitle.Of(x))
        );
        Poll poll = new Poll(id, question, optionsMap, pollAnswerType);
        return poll;
    }

    public void Vote(Ytsoober voter, Option option)
    {
        if (_options.All(x => x.Id != option.Id))
        {
            throw new ArgumentException("Poll doesn't have this option");
        }

        TotalCountPoll = TotalCountPoll.Of(TotalCountPoll.Value + 1);
        option.Vote(voter);
        RecalculateFictions();
    }

    public void Unvote(Ytsoober voter, Option option)
    {
        if (_options.All(x => x.Id != option.Id))
        {
            throw new ArgumentException("Poll doesn't have this option");
        }

        TotalCountPoll = TotalCountPoll.Of(TotalCountPoll.Value - 1);
        option.Unvote(voter);
        RecalculateFictions();
    }

    private void RecalculateFictions()
    {
        foreach (var option in _options)
        {
            option.RecalculateFiction(TotalCountPoll);
        }
    }
}
