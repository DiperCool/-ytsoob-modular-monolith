using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Messaging;
using BuildingBlocks.Abstractions.Messaging.Context;
using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Core.Messaging;
using Ytsoob.Modules.Ytsoobers.Ytsoobers.Features.CreatingYtsoober.v1.CreateYtsoober;

namespace Ytsoob.Modules.Ytsoobers.Users.Features.RegisteringUser.v1.Events.Integration.External;

public record UserRegistered(
    Guid IdentityId,
    long YtsooberId,
    string Username,
    string Email,
    string PhoneNumber,
    List<string>? Roles
) : IntegrationEvent, ITxRequest;

public class UserRegisteredConsumer : IMessageHandler<UserRegistered>
{
    private readonly ICommandProcessor _commandProcessor;

    public UserRegisteredConsumer(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    public async Task HandleAsync(
        IConsumeContext<UserRegistered> messageContext,
        CancellationToken cancellationToken = default
    )
    {
        var userRegistered = messageContext.Message;
        await _commandProcessor.SendAsync(
            new CreateYtsoober(
                userRegistered.YtsooberId,
                userRegistered.IdentityId,
                userRegistered.Username,
                userRegistered.Email
            ),
            cancellationToken
        );
    }
}
