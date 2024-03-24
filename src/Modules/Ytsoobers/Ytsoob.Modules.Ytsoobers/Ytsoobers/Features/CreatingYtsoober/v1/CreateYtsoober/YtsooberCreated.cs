using BuildingBlocks.Core.CQRS.Event.Internal;
using BuildingBlocks.Core.Messaging;
using Ytsoob.Modules.Ytsoobers.Profiles.Dtos.v1;

namespace Ytsoob.Modules.Ytsoobers.Ytsoobers.Features.CreatingYtsoober.v1.CreateYtsoober;

public record YtsooberCreated(long Id, Guid IdentityId, string? Username, string Email, ProfileDto Profile)
    : DomainEvent;
