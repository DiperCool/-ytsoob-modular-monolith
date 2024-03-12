using Ytsoob.Modules.Identity.Shared.Models;

namespace Ytsoob.Modules.Identity.Identity.Features.RefreshingToken.v1;

public record RefreshTokenResponse
{
    public RefreshTokenResponse(ApplicationUser user, string accessToken, string refreshToken)
    {
        UserId = user.Id;
        Username = user.UserName!;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public string AccessToken { get; }
    public Guid UserId { get; }
    public string Username { get; }
    public string RefreshToken { get; }
}
