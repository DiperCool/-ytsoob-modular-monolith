using Ardalis.GuardClauses;

namespace Ytsoob.Modules.Subscriptions.Subscriptions.ValueObjects;

public class Title
{
    protected Title(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }

    public static Title Of(string value)
    {
        Guard.Against.NullOrWhiteSpace(value);
        if (value.Length > 50)
        {
            throw new ArgumentException("Value exceed limit");
        }

        return new Title(value);
    }

    public static implicit operator string(Title value) => value.Value;
}
