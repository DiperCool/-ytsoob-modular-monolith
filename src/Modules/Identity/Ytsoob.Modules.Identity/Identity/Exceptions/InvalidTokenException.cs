using System.Security.Claims;
using BuildingBlocks.Core.Exception.Types;

namespace Ytsoob.Modules.Identity.Identity.Exceptions;

public class InvalidTokenException : AppException
{
    public InvalidTokenException(ClaimsPrincipal? claimsPrincipal)
        : base("access_token is invalid!") { }
}
