using BuildingBlocks.Core.Domain;
using Ytsoob.Modules.Posts.Reactions.Enums;
using Ytsoob.Modules.Posts.Reactions.ValueObjects;

namespace Ytsoob.Modules.Posts.Reactions.Models;

public class ReactionStats : Entity<long>
{
    public ReactionNumber Like { get; private set; } = default!;
    public ReactionNumber Dislike { get; private set; } = default!;
    public ReactionNumber Angry { get; private set; } = default!;
    public ReactionNumber Happy { get; private set; } = default!;
    public ReactionNumber Wonder { get; private set; } = default!;
    public ReactionNumber Crying { get; private set; } = default!;

    // ef core
    protected ReactionStats() { }

    protected ReactionStats(long id)
    {
        Id = id;
        Like = ReactionNumber.Of(0);
        Dislike = ReactionNumber.Of(0);
        Angry = ReactionNumber.Of(0);
        Happy = ReactionNumber.Of(0);
        Wonder = ReactionNumber.Of(0);
        Crying = ReactionNumber.Of(0);
    }

    public static ReactionStats Create(long id)
    {
        return new ReactionStats(id);
    }

    public void AddReaction(ReactionType type)
    {
        switch (type)
        {
            case ReactionType.Like:
                Like = ReactionNumber.Of(Like + 1);
                break;
            case ReactionType.Dislike:
                Dislike = ReactionNumber.Of(Dislike + 1);
                break;
            case ReactionType.Angry:
                Angry = ReactionNumber.Of(Angry + 1);
                break;
            case ReactionType.Happy:
                Happy = ReactionNumber.Of(Happy + 1);
                break;
            case ReactionType.Wonder:
                Wonder = ReactionNumber.Of(Wonder + 1);
                break;
            case ReactionType.Crying:
                Crying = ReactionNumber.Of(Crying + 1);
                break;
            default:
                throw new ArgumentException("Invalid ReactionType.");
        }
    }

    public void RemoveReaction(ReactionType type)
    {
        switch (type)
        {
            case ReactionType.Like:
                Like = ReactionNumber.Of(Like - 1);
                break;
            case ReactionType.Dislike:
                Dislike = ReactionNumber.Of(Dislike - 1);
                break;
            case ReactionType.Angry:
                Angry = ReactionNumber.Of(Angry - 1);
                break;
            case ReactionType.Happy:
                Happy = ReactionNumber.Of(Happy - 1);
                break;
            case ReactionType.Wonder:
                Wonder = ReactionNumber.Of(Wonder - 1);
                break;
            case ReactionType.Crying:
                Crying = ReactionNumber.Of(Crying - 1);
                break;
            default:
                throw new ArgumentException("Invalid ReactionType.");
        }
    }
}
