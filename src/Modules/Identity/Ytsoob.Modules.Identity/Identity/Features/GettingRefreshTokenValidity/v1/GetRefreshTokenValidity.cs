using BuildingBlocks.Abstractions.CQRS.Query;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Identity.Shared.Data;

namespace Ytsoob.Modules.Identity.Identity.Features.GettingRefreshTokenValidity.v1;

public record GetRefreshTokenValidity(Guid UserId, string RefreshToken) : IQuery<bool>;

public class GetRefreshTokenValidityQueryHandler : IQueryHandler<GetRefreshTokenValidity, bool>
{
    private readonly IdentityContext _context;

    public GetRefreshTokenValidityQueryHandler(IdentityContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(GetRefreshTokenValidity request, CancellationToken cancellationToken)
    {
        var refreshToken = await _context
            .Set<Shared.Models.RefreshToken>()
            .FirstOrDefaultAsync(
                rt => rt.UserId == request.UserId && rt.Token == request.RefreshToken,
                cancellationToken
            );

        if (refreshToken == null)
        {
            return false;
        }

        if (!refreshToken.IsRefreshTokenValid())
            return false;

        return true;
    }
}
