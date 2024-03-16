using BuildingBlocks.Core.CQRS.Event.Internal;
using Ytsoob.Modules.Ytsoobers.Profiles.ValueObjects;
using Ytsoob.Modules.Ytsoobers.Ytsoobers.ValueObjects;

namespace Ytsoob.Modules.Ytsoobers.Profiles.Features.UpdatingProfile.v1.UpdateProfile;

public record YtsooberProfileUpdated(
  YtsooberId Id,
  FirstName FirstName,
  LastName LastName,
  string? Avatar
) : DomainEvent;
