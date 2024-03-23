using Ardalis.GuardClauses;

namespace Ytsoob.Modules.Posts.Comments.ValueObjects;

public class CommentContent
{
    protected CommentContent(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }

    public static CommentContent Of(string value)
    {
        Guard.Against.NullOrWhiteSpace(value);
        if (value.Length > 150)
        {
            throw new ArgumentException("Value exceed limit");
        }

        return new CommentContent(value);
    }

    public static implicit operator string(CommentContent value) => value.Value;
}
