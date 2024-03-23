using Ardalis.GuardClauses;

namespace Ytsoob.Modules.Posts.Polls.ValueObjects;

public class OptionTitle
{
    protected OptionTitle(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }

    public static OptionTitle Of(string value)
    {
        Guard.Against.NullOrWhiteSpace(value);
        if (value.Length > 30)
        {
            throw new ArgumentException("Value exceed limit");
        }

        return new OptionTitle(value);
    }

    public static implicit operator string(OptionTitle value) => value.Value;
}
