using BuildingBlocks.Core.Domain;
using Ytsoob.Modules.Subscriptions.Subscriptions.Features.CreatingSubscription.v1.CreateSubscription;
using Ytsoob.Modules.Subscriptions.Subscriptions.Features.RemovingSubscription.v1.RemoveSubscription;
using Ytsoob.Modules.Subscriptions.Subscriptions.Features.UpdatingSubsctiption.v1.UpdateSubsctiption;
using Ytsoob.Modules.Subscriptions.Subscriptions.ValueObjects;
using Ytsoob.Modules.Subscriptions.Ytsoobers.Models;

namespace Ytsoob.Modules.Subscriptions.Subscriptions.Models;

public class Subscription : Aggregate<SubscriptionId>
{
    public Title Title { get; private set; } = default!;
    public Description Description { get; private set; } = default!;
    public string? Photo { get; private set; }
    public Price Price { get; private set; }
    public Ytsoober Ytsoober { get; set; } = default!;
    public long YtsooberId { get; set; }

    // ef core
    protected Subscription() { }

    protected Subscription(SubscriptionId id, Title title, Description description, Price price, long ytsooberId) =>
        (Id, Title, Description, Price, YtsooberId) = (id, title, description, price, ytsooberId);

    public static Subscription Create(
        SubscriptionId id,
        Title title,
        Description description,
        Price price,
        long ytsooberId
    )
    {
        Subscription subscription = new Subscription(id, title, description, price, ytsooberId);
        subscription.AddDomainEvents(
            new SubscriptionCreatedDomainEvent(id, title, description, null, price, ytsooberId)
        );
        return subscription;
    }

    public void Update(Title title, Description description, string? photo)
    {
        (Title, Description, Photo) = (title, description, photo);
        AddDomainEvents(new SubscriptionUpdatedDomainEvent(Id, Title, Description, Photo));
    }

    public void Remove()
    {
        AddDomainEvents(new SubscriptionRemovedDomainEvent(Id));
    }
}
