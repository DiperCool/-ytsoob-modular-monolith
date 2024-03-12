using System.Security.Claims;

namespace BuildingBlocks.Security.Jwt;

public interface IJwtService
{
    GenerateTokenResult GenerateJwtToken(
        string userName,
        string email,
        string userId,
        string ytsooberId,
        bool? isVerified = null,
        string? refreshToken = null,
        IReadOnlyList<Claim>? usersClaims = null,
        IReadOnlyList<string>? rolesClaims = null,
        IReadOnlyList<string>? permissionsClaims = null
    );

    ClaimsPrincipal? GetPrincipalFromToken(string token);
}
