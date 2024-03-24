using BuildingBlocks.Abstractions.CQRS.Event;
using BuildingBlocks.Abstractions.CQRS.Event.Internal;
using BuildingBlocks.Abstractions.Messaging;
using Ytsoob.Modules.Ytsoobers.Ytsoobers.Features.CreatingYtsoober.v1.CreateYtsoober;

namespace Ytsoob.Modules.Ytsoobers;

public class YtsooberEventMapper : IIntegrationEventMapper
{
    public IReadOnlyList<IIntegrationEvent?>? MapToIntegrationEvents(IReadOnlyList<IDomainEvent> domainEvents)
    {
        return domainEvents.Select(MapToIntegrationEvent).ToList().AsReadOnly();
    }

    public IIntegrationEvent? MapToIntegrationEvent(IDomainEvent domainEvent)
    {
        return domainEvent switch
        {
            YtsooberCreated e
                => new YtsooberCreatedV1(
                    e.Id,
                    e.IdentityId,
                    e.Username,
                    e.Email,
                    new YtsooberCreatedProfileV1(e.Profile.FirstName, e.Profile.LastName, e.Profile.Avatar)
                ),
            _ => null
        };
    }
}
