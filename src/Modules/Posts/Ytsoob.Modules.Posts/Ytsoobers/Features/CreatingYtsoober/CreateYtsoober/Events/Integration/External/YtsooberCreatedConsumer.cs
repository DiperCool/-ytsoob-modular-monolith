namespace Ytsoob.Modules.Posts.Ytsoobers.Features.CreatingYtsoober.CreateYtsoober.Events.Integration.External;







// public class YtsooberCreatedConsumer : IConsumer<YtsooberCreatedV1>
// {
//     private ICommandProcessor _commandProcessor;
//
//     public YtsooberCreatedConsumer(ICommandProcessor commandProcessor)
//     {
//         _commandProcessor = commandProcessor;
//     }
//
//     public async Task Consume(ConsumeContext<YtsooberCreatedV1> context)
//     {
//         var message = context.Message;
//         await _commandProcessor.SendAsync(
//             new v1.CreateYtsoober(message.Id, message.IdentityId, message.Username, message.Email, message.Profile.Avatar)
//         );
//     }
// }
