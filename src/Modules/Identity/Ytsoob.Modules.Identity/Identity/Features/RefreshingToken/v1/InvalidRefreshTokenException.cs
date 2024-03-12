using BuildingBlocks.Core.Exception.Types;

namespace Ytsoob.Modules.Identity.Identity.Features.RefreshingToken.v1;

public class InvalidRefreshTokenException : BadRequestException
{
    public InvalidRefreshTokenException(Shared.Models.RefreshToken? refreshToken)
        : base($"refresh token {refreshToken?.Token} is invalid!") { }
}
