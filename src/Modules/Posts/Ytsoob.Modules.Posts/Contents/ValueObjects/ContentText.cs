using Ardalis.GuardClauses;

namespace Ytsoob.Modules.Posts.Contents.ValueObjects;

public class ContentText
{
    public ContentText(string value)
    {
        Value = value;
    }
    public string Value { get; private set; }

    public static ContentText Of(string value)
    {
        Guard.Against.NullOrWhiteSpace(value);
        return new ContentText(value);
    }

    public static implicit operator string(ContentText value) => value.Value;
}
