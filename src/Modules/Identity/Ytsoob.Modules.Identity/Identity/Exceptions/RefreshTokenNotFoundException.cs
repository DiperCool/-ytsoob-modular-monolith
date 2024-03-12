using System.Net;
using BuildingBlocks.Core.Exception.Types;
using Ytsoob.Modules.Identity.Shared.Models;

namespace Ytsoob.Modules.Identity.Identity.Exceptions;

public class RefreshTokenNotFoundException : AppException
{
    public RefreshTokenNotFoundException(RefreshToken? refreshToken)
        : base("Refresh token not found.", HttpStatusCode.NotFound) { }
}
