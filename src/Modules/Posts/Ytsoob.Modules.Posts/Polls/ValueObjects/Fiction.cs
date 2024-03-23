namespace Ytsoob.Modules.Posts.Polls.ValueObjects;

public class Fiction
{
    protected Fiction(decimal value)
    {
        Value = value;
    }

    public decimal Value { get; private set; }

    public static Fiction Of(decimal value)
    {
        if (value < 0)
        {
            throw new ArgumentException("Negative number");
        }

        return new Fiction(value);
    }

    public static implicit operator decimal(Fiction value) => value.Value;
}
