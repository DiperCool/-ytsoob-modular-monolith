using BuildingBlocks.Abstractions.CQRS.Query;
using BuildingBlocks.Abstractions.Web;
using Hellang.Middleware.ProblemDetails;
using Ytsoob.Modules.Identity.Users.Features.RegisteringUser.v1;

namespace Ytsoob.Modules.Identity.Users.Features.GettingUerByEmail.v1;

// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing
// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis
public static class GetUserByEmailEndpoint
{
    internal static RouteHandlerBuilder MapGetUserByEmailEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/by-email/{email}", GetUserByEmail)
            .AllowAnonymous()
            .WithTags(UsersConfigs.Tag)
            .Produces<RegisterUserResponse>(StatusCodes.Status200OK)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status404NotFound)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName("GetUserByEmail")
            .WithDisplayName("Get User by email.")
            .WithOpenApi(
                operation =>
                    new(operation) { Description = "Getting User by email.", Summary = "Getting User by email." }
            );
    }

    private static async Task<IResult> GetUserByEmail(
        [FromRoute] string email,
        IGatewayProcessor<IdentityModuleConfiguration> queryProcessor,
        CancellationToken cancellationToken
    )
    {
        return await queryProcessor.ExecuteQuery(async processor =>
        {

            var result = await processor.SendAsync(new GetUserByEmail(email), cancellationToken);

            return Results.Ok(result);
        });
    }
}
