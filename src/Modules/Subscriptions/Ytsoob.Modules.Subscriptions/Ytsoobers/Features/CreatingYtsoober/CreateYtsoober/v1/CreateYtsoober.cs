using BuildingBlocks.Abstractions.CQRS.Command;
using MediatR;
using Microsoft.Extensions.Logging;
using Ytsoob.Modules.Subscriptions.Shared.Contracts;
using Ytsoob.Modules.Subscriptions.Ytsoobers.Models;

namespace Ytsoob.Modules.Subscriptions.Ytsoobers.Features.CreatingYtsoober.CreateYtsoober.v1;

public record CreateYtsoober(long Id, Guid IdentityId, string? Username, string Email, string Avatar) : ITxCommand;

internal class CreateYtsooberHandler : ICommandHandler<CreateYtsoober>
{
    private ILogger<CreateYtsooberHandler> _logger;
    private ISubscriptionsDbContext _subscriptionsDbContext;

    public CreateYtsooberHandler(ILogger<CreateYtsooberHandler> logger, ISubscriptionsDbContext subscriptionsDbContext)
    {
        _logger = logger;
        _subscriptionsDbContext = subscriptionsDbContext;
    }

    public async Task<Unit> Handle(CreateYtsoober request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating ytsoober");
        Ytsoober ytsoober = new Ytsoober()
        {
            Id = request.Id,
            Email = request.Email,
            Username = request.Username,
            IdentityId = request.IdentityId,
            Avatar = request.Avatar
        };
        await _subscriptionsDbContext.Ytsoobers.AddAsync(ytsoober, cancellationToken);
        await _subscriptionsDbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Ytsoober created with ID = {ID}", ytsoober.Id);
        return Unit.Value;
    }
}
