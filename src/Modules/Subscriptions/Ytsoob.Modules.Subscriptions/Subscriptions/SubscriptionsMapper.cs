using AutoMapper;
using Ytsoob.Modules.Subscriptions.Subscriptions.Dtos;
using Ytsoob.Modules.Subscriptions.Subscriptions.Models;

namespace Ytsoob.Modules.Subscriptions.Subscriptions;

public class SubscriptionsMapper : Profile
{
    public SubscriptionsMapper()
    {
        CreateMap<Subscription, SubscriptionDto>()
            .ForMember(x => x.Id, expression => expression.MapFrom(x => x.Id.Value))
            .ForMember(x => x.Description, expression => expression.MapFrom(x => x.Description.Value))
            .ForMember(x => x.Price, expression => expression.MapFrom(x => x.Price.Value))
            .ForMember(x => x.Title, expression => expression.MapFrom(x => x.Title.Value));
    }
}
