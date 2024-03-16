using Ardalis.GuardClauses;

namespace Ytsoob.Modules.Ytsoobers.Ytsoobers.ValueObjects;

public class Username
{
    protected Username(string value)
    {
        Value = value;
    }

    protected Username() { }

    public string Value { get; }

    public static Username Of(string value)
    {
        Guard.Against.NullOrWhiteSpace(value);
        return new Username(value);
    }

    public static implicit operator string(Username value) => value.Value;
}
