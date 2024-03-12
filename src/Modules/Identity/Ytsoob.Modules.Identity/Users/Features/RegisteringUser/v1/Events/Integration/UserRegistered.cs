using BuildingBlocks.Core.Messaging;

namespace Ytsoob.Modules.Identity.Users.Features.RegisteringUser.v1.Events.Integration;

public record UserRegistered(
    Guid IdentityId,
    long YtsooberId,
    string Username,
    string Email,
    string PhoneNumber,
    List<string>? Roles
) : IntegrationEvent;
