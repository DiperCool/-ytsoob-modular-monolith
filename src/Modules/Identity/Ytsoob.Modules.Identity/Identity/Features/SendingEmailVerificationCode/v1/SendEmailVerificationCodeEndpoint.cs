using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Web;
using Hellang.Middleware.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;
using Ytsoob.Modules.Identity.Users;

namespace Ytsoob.Modules.Identity.Identity.Features.SendingEmailVerificationCode.v1;

public static class SendEmailVerificationCodeEndpoint
{
    internal static RouteHandlerBuilder MapSendEmailVerificationCodeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/send-email-verification-code", SendEmailVerificationCode)
            .AllowAnonymous()
            .WithTags(IdentityConfigs.Tag)
            .Produces(StatusCodes.Status200OK)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName("SendEmailVerificationCode")
            .WithDisplayName("Send Email Verification Code.")
            .WithMetadata(
                new SwaggerOperationAttribute("Sending Email Verification Code.", "Sending Email Verification Code.")
            );
    }

    private static async Task<IResult> SendEmailVerificationCode(
        SendEmailVerificationCodeRequest request,
        IGatewayProcessor<IdentityModuleConfiguration> commandProcessor,
        CancellationToken cancellationToken
    )
    {
        return await commandProcessor.ExecuteCommand(async processor =>
        {
            var command = new SendEmailVerificationCode(request.Email);

            await processor.SendAsync(command, cancellationToken);

            return Results.Ok();
        });
    }
}
