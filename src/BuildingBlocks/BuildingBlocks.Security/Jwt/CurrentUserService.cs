using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BuildingBlocks.Security.Jwt;

public class CurrentUserService : ICurrentUserService
{
  private readonly ILogger<CurrentUserService> _logger;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public CurrentUserService(
    IHttpContextAccessor httpContextAccessor,
    ILogger<CurrentUserService> logger
  )
  {
    _httpContextAccessor =
      httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
  }

  public string? UserId
  {
    get
    {
      var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue("sub");
      return userId;
    }
  }

  public Guid UserIdGuid
  {
    get { return UserId == null ? Guid.Empty : Guid.Parse(UserId); }
  }

  public long YtsooberId
  {
    get
    {
      var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(
        JwtRegisteredClaimNames.NameId
      );
      return long.Parse(userId);
    }
  }

  public string? JwtToken
  {
    get { return _httpContextAccessor.HttpContext?.Request?.Headers["Authorization"]; }
  }

  public bool IsAuthenticated
  {
    get
    {
      var isAuthenticated = _httpContextAccessor
        .HttpContext?.User?.Identities?.FirstOrDefault()
        ?.IsAuthenticated;
      return isAuthenticated.HasValue && isAuthenticated.Value;
    }
  }

  public string Role
  {
    get
    {
      var role = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
      return role;
    }
  }
}
