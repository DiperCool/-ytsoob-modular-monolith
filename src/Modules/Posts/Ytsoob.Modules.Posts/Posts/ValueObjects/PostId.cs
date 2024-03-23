using BuildingBlocks.Abstractions.Domain;

namespace Ytsoob.Modules.Posts.Posts.ValueObjects;

public record PostId : AggregateId
{
    protected PostId(long value) : base(value)
    {
    }

    public static PostId Of(long value) => new(value);
}
