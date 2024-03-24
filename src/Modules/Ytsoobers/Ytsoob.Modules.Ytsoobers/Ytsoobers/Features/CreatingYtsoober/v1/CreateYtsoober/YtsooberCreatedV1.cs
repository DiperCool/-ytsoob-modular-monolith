using BuildingBlocks.Core.Messaging;

namespace Ytsoob.Modules.Ytsoobers.Ytsoobers.Features.CreatingYtsoober.v1.CreateYtsoober;

public record YtsooberCreatedProfileV1(string FirstName, string LastName, string Avatar);

public record YtsooberCreatedV1(
    long Id,
    Guid IdentityId,
    string? Username,
    string Email,
    YtsooberCreatedProfileV1 Profile
) : IntegrationEvent;
