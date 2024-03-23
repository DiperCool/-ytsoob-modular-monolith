using BuildingBlocks.Abstractions.Domain;

namespace Ytsoob.Modules.Posts.Reactions.ValueObjects;

public record ReactionId : AggregateId
{
    protected ReactionId(long value)
        : base(value) { }

    public static ReactionId Of(long value) => new(value);
}
