using Ardalis.GuardClauses;

namespace Ytsoob.Modules.Subscriptions.Subscriptions.ValueObjects;

public class Description
{
    protected Description(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }

    public static Description Of(string value)
    {
        Guard.Against.NullOrWhiteSpace(value);
        if (value.Length > 300)
        {
            throw new ArgumentException("Value exceed limit");
        }

        return new Description(value);
    }

    public static implicit operator string(Description value) => value.Value;
}
