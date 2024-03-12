using BuildingBlocks.Core.Messaging;
using Ytsoob.Modules.Identity.Shared.Models;

namespace Ytsoob.Modules.Identity.Users.Features.UpdatingUserState.v1.Events.Integration;

public record UserStateUpdated(Guid UserId, UserState OldUserState, UserState NewUserState) : IntegrationEvent;
