using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Messaging;
using BuildingBlocks.Abstractions.Messaging.Context;
using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Abstractions.Web;
using BuildingBlocks.Core.Messaging;
using BuildingBlocks.Web.Module;
using DotNetCore.CAP;
using MediatR;
using Ytsoob.Modules.Ytsoobers.Ytsoobers.Features.CreatingYtsoober.v1.CreateYtsoober;
using Ytsoob.Modules.Ytsoobers.Ytsoobers.ValueObjects;

namespace Ytsoob.Modules.Ytsoobers.Users.Features.RegisteringUser.v1.Events.Integration.External;

public record UserRegistered(
    Guid IdentityId,
    long YtsooberId,
    string Username,
    string Email,
    string PhoneNumber,
    List<string>? Roles
) : IntegrationEvent, ITxRequest;

public class UserRegisteredConsumer : ICapSubscribe
{
    private IGatewayProcessor<YtsoobersModuleConfiguration> _commandProcessor;

    public UserRegisteredConsumer(IGatewayProcessor<YtsoobersModuleConfiguration> commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    [CapSubscribe(nameof(UserRegistered))]
    public async Task HandleAsync(UserRegistered userRegistered, CancellationToken cancellationToken = default)
    {
        await _commandProcessor.ExecuteCommand<CreateYtsoober, Unit>(
            new CreateYtsoober(
                YtsooberId.Of(userRegistered.YtsooberId),
                userRegistered.IdentityId,
                userRegistered.Username,
                userRegistered.Email
            ),
            cancellationToken
        );
    }
}
