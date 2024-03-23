namespace Ytsoob.Modules.Posts.Reactions.ValueObjects;

public class ReactionNumber
{
    protected ReactionNumber(long value)
    {
        Value = value;
    }

    public long Value { get; private set; }

    public static ReactionNumber Of(long value)
    {
        if (value < 0)
        {
            throw new ArgumentException("Negative number");
        }

        return new ReactionNumber(value);
    }

    public static implicit operator long(ReactionNumber value) => value.Value;
}
