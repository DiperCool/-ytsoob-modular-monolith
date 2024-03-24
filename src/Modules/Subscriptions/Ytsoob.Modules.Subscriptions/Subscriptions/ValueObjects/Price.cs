namespace Ytsoob.Modules.Subscriptions.Subscriptions.ValueObjects;

public class Price
{
    protected Price(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; private set; }

    public static Price Of(decimal value)
    {
        if (value < 0)
        {
            throw new ArgumentException("Negative number");
        }

        return new Price(value);
    }

    public static implicit operator decimal(Price value) => value.Value;
}
