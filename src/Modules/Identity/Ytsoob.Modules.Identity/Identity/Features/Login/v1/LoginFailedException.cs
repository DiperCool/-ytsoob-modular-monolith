using BuildingBlocks.Core.Exception.Types;

namespace Ytsoob.Modules.Identity.Identity.Features.Login.v1;

public class LoginFailedException : AppException
{
    public LoginFailedException(string userNameOrEmail)
        : base($"Login failed for username: {userNameOrEmail}")
    {
        UserNameOrEmail = userNameOrEmail;
    }

    public string UserNameOrEmail { get; }
}
