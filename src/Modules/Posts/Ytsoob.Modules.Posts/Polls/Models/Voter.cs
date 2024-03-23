using BuildingBlocks.Core.Domain;
using Ytsoob.Modules.Posts.Polls.ValueObjects;
using Ytsoob.Modules.Posts.Ytsoobers.Models;

namespace Ytsoob.Modules.Posts.Polls.Models;

public class Voter : Entity<long>
{
    public long YtsooberId { get; set; }
    public Ytsoober Ytsoober { get; set; } = null!;

    public OptionId OptionId { get; set; }
    public Option Option { get; set; } = null!;

    protected Voter() { }

    public Voter(long id, long ytsooberId, OptionId optionId)
    {
        Id = id;
        YtsooberId = ytsooberId;
        OptionId = optionId;
    }
}
