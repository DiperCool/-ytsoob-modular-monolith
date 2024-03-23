using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Messaging;
using BuildingBlocks.Abstractions.Messaging.Context;
using BuildingBlocks.Abstractions.Persistence;
using BuildingBlocks.Core.Messaging;
using BuildingBlocks.Web.Module;
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

public class UserRegisteredConsumer : IMessageHandler<UserRegistered>
{
    private readonly IServiceProvider _serviceProvider;

    public UserRegisteredConsumer(IServiceProvider serviceProvider)
    {
        _serviceProvider =
            CompositionRootRegistry.GetByModule<YtsoobersModuleConfiguration>()?.ServiceProvider ?? serviceProvider;
    }

    public async Task HandleAsync(
        IConsumeContext<UserRegistered> messageContext,
        CancellationToken cancellationToken = default
    )
    {
        var userRegistered = messageContext.Message;
        var scope = _serviceProvider.CreateScope();
        var _commandProcessor = scope.ServiceProvider.GetRequiredService<ICommandProcessor>();
        await _commandProcessor.SendAsync(
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
