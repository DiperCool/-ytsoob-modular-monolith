using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.CQRS.Command;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Identity.Identity.Exceptions;
using Ytsoob.Modules.Identity.Identity.Features.RefreshingToken.v1;
using Ytsoob.Modules.Identity.Shared.Data;

namespace Ytsoob.Modules.Identity.Identity.Features.RevokingRefreshToken.v1;

public record RevokeRefreshToken(string RefreshToken) : ICommand;

internal class RevokeRefreshTokenHandler : ICommandHandler<RevokeRefreshToken>
{
    private readonly IdentityContext _context;

    public RevokeRefreshTokenHandler(IdentityContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RevokeRefreshToken request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request, nameof(RevokeRefreshToken));

        var refreshToken = await _context
            .Set<global::Ytsoob.Modules.Identity.Shared.Models.RefreshToken>()
            .FirstOrDefaultAsync(x => x.Token == request.RefreshToken, cancellationToken: cancellationToken);

        if (refreshToken == null)
            throw new RefreshTokenNotFoundException(refreshToken);

        if (!refreshToken.IsRefreshTokenValid())
            throw new InvalidRefreshTokenException(refreshToken);

        // revoke token and save
        refreshToken.RevokedAt = DateTime.Now;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
