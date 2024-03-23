namespace Ytsoob.Modules.Posts.Polls.ValueObjects;

public class TotalCountPoll
{
    protected TotalCountPoll(long value)
    {
        Value = value;
    }

    public long Value { get; private set; }

    public static TotalCountPoll Of(long value)
    {
        if (value < 0)
        {
            throw new ArgumentException("Negative number");
        }

        return new TotalCountPoll(value);
    }

    public static implicit operator long(TotalCountPoll value) => value.Value;
}
