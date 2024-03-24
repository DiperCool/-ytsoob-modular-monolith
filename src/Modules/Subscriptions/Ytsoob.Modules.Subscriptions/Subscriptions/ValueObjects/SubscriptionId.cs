using BuildingBlocks.Abstractions.Domain;

namespace Ytsoob.Modules.Subscriptions.Subscriptions.ValueObjects;

public record SubscriptionId : AggregateId
{
    protected SubscriptionId(long value)
        : base(value) { }

    public static SubscriptionId Of(long value) => new(value);
}
