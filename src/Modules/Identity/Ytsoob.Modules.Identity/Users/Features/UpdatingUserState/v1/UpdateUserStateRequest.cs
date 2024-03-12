using Ytsoob.Modules.Identity.Shared.Models;

namespace Ytsoob.Modules.Identity.Users.Features.UpdatingUserState.v1;

public record UpdateUserStateRequest
{
    public UserState UserState { get; init; }
}
