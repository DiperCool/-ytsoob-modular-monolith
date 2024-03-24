using BuildingBlocks.Abstractions.CQRS.Event;
using BuildingBlocks.Abstractions.CQRS.Event.Internal;
using BuildingBlocks.Abstractions.Messaging;
using Ytsoob.Modules.Subscriptions.Subscriptions.Features.CreatingSubscription.v1.CreateSubscription;
using Ytsoob.Modules.Subscriptions.Subscriptions.Features.RemovingSubscription.v1.RemoveSubscription;
using Ytsoob.Modules.Subscriptions.Subscriptions.Features.UpdatingSubsctiption.v1.UpdateSubsctiption;

namespace Ytsoob.Modules.Subscriptions.Subscriptions;



// public class SubscriptionsEventMapper : IIntegrationEventMapper
// {
//     public IReadOnlyList<IIntegrationEvent?>? MapToIntegrationEvents(IReadOnlyList<IDomainEvent> domainEvents)
//     {
//         return domainEvents.Select(MapToIntegrationEvent).ToList().AsReadOnly();
//     }
//
//     public IIntegrationEvent? MapToIntegrationEvent(IDomainEvent domainEvent)
//     {
//         return domainEvent switch
//         {
//             SubscriptionCreatedDomainEvent e
//                 => new SubscriptionCreatedV1(e.Id, e.Title, e.Description, e.Photo, e.Price, e.YtsooberId),
//             SubscriptionUpdatedDomainEvent e => new SubscriptionUpdatedV1(e.Id, e.Title, e.Description, e.Photo),
//             SubscriptionRemovedDomainEvent e => new SubscriptionRemovedV1(e.Id),
//             _ => null
//         };
//     }
// }
