using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Web;
using Hellang.Middleware.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;
using Ytsoob.Modules.Identity.Users;

namespace Ytsoob.Modules.Identity.Identity.Features.VerifyingEmail.v1;

public static class VerifyEmailEndpoint
{
    internal static RouteHandlerBuilder MapSendVerifyEmailEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/verify-email", VerifyEmail)
            .AllowAnonymous()
            .WithTags(IdentityConfigs.Tag)
            .Produces(StatusCodes.Status200OK)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status409Conflict)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status500InternalServerError)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName("VerifyEmail")
            .WithDisplayName("Verify Email.")
            .WithMetadata(new SwaggerOperationAttribute("Verifying Email", "Verifying Email"));
    }

    private static async Task<IResult> VerifyEmail(
        VerifyEmailRequest request,
        IGatewayProcessor<IdentityModuleConfiguration> commandProcessor,
        CancellationToken cancellationToken
    )
    {
        return await commandProcessor.ExecuteCommand(async processor =>
        {
            var command = new VerifyEmail(request.Email, request.Code);

            await processor.SendAsync(command, cancellationToken);

            return Results.Ok();
        });
    }
}
