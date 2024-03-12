using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Web;
using Hellang.Middleware.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;
using Ytsoob.Modules.Identity.Users;

namespace Ytsoob.Modules.Identity.Identity.Features.RefreshingToken.v1;

public static class RefreshTokenEndpoint
{
    internal static RouteHandlerBuilder MapRefreshTokenEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/refresh-token", RefreshToken)
            .RequireAuthorization()
            .WithTags(IdentityConfigs.Tag)
            .Produces<RefreshTokenResponse>()
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName("RefreshToken")
            .WithDisplayName("Refresh Token.")
            .WithMetadata(new SwaggerOperationAttribute("Refreshing Token", "Refreshing Token"));
    }

    private static async Task<IResult> RefreshToken(
        RefreshTokenRequest request,
        IGatewayProcessor<IdentityModuleConfiguration> commandProcessor,
        CancellationToken cancellationToken
    )
    {
        return await commandProcessor.ExecuteCommand(async processor =>
        {
            var command = new RefreshToken(request.AccessToken, request.RefreshToken);

            var result = await processor.SendAsync(command, cancellationToken);

            return Results.Ok(result);
        });
    }
}
