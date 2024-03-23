using Ardalis.GuardClauses;

namespace Ytsoob.Modules.Posts.Polls.ValueObjects;

public class Question
{
    protected Question(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }

    public static Question Of(string value)
    {
        Guard.Against.NullOrWhiteSpace(value);
        if (value.Length > 30)
        {
            throw new ArgumentException("Value exceed limit");
        }

        return new Question(value);
    }

    public static implicit operator string(Question value) => value.Value;
}
