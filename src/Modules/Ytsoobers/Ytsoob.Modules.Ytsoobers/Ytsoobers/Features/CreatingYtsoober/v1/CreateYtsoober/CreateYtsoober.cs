using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Core.Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Ytsoobers.Shared.Data;
using Ytsoob.Modules.Ytsoobers.Ytsoobers.Models;
using Ytsoob.Modules.Ytsoobers.Ytsoobers.ValueObjects;

namespace Ytsoob.Modules.Ytsoobers.Ytsoobers.Features.CreatingYtsoober.v1.CreateYtsoober;

public record CreateYtsoober(YtsooberId YtsooberId, Guid IdentityId, string Username, string Email) : ITxCreateCommand;

public class CreateYtsooberHandler : ICommandHandler<CreateYtsoober>
{
    private YtsoobersDbContext _ytsoobersDbDbContext;

    public CreateYtsooberHandler(YtsoobersDbContext ytsoobersDbDbContext)
    {
        _ytsoobersDbDbContext = ytsoobersDbDbContext;
    }

    public async Task<Unit> Handle(CreateYtsoober request, CancellationToken cancellationToken)
    {
        if (
            await _ytsoobersDbDbContext.Ytsoobers.AnyAsync(
                x => x.Id == request.YtsooberId,
                cancellationToken: cancellationToken
            )
        )
            return Unit.Value;
        Ytsoober ytsoober = Ytsoober.Create(
            YtsooberId.Of(request.YtsooberId),
            Username.Of(request.Username),
            Email.Of(request.Email),
            request.IdentityId
        );
        await _ytsoobersDbDbContext.Ytsoobers.AddAsync(ytsoober, cancellationToken);
        await _ytsoobersDbDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
