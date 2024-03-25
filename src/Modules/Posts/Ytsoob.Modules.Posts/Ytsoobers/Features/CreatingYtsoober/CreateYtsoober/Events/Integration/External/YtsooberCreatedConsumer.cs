using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Messaging;
using BuildingBlocks.Abstractions.Messaging.Context;
using BuildingBlocks.Core.Messaging;
using BuildingBlocks.Core.Messaging.Context;
using BuildingBlocks.Web.Module;

namespace Ytsoob.Modules.Posts.Ytsoobers.Features.CreatingYtsoober.CreateYtsoober.Events.Integration.External;

public record YtsooberCreatedProfileV1(string FirstName, string LastName, string Avatar);

public record YtsooberCreatedV1(
    long Id,
    Guid IdentityId,
    string? Username,
    string Email,
    YtsooberCreatedProfileV1 Profile
) : IntegrationEvent;

public class YtsooberCreatedConsumer : IMessageHandler<YtsooberCreatedV1>
{
    private ICommandProcessor _commandProcessor;

    public YtsooberCreatedConsumer(ICommandProcessor commandProcessor)
    {
        _commandProcessor = commandProcessor;
    }

    public async Task HandleAsync(
        IConsumeContext<YtsooberCreatedV1> messageContext,
        CancellationToken cancellationToken = default
    )
    {
        var userRegistered = messageContext.Message;
        await _commandProcessor.SendAsync(
            new v1.CreateYtsoober(
                userRegistered.Id,
                userRegistered.IdentityId,
                userRegistered.Username,
                userRegistered.Email,
                userRegistered.Profile.Avatar
            ),
            cancellationToken
        );
    }
}
