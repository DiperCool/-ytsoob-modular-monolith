using Ytsoob.Modules.Identity.Shared.Models;

namespace Ytsoob.Modules.Identity.Identity.Features.Login.v1;

public record LoginResponse
{
    public LoginResponse(ApplicationUser user, string accessToken, string refreshToken)
    {
        UserId = user.Id;
        Username = user.UserName;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public Guid UserId { get; }
    public string AccessToken { get; }
    public string Username { get; }
    public string RefreshToken { get; }
}
