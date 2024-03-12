using BuildingBlocks.Abstractions.CQRS.Query;
using BuildingBlocks.Abstractions.Web;
using Hellang.Middleware.ProblemDetails;

namespace Ytsoob.Modules.Identity.Identity.Features.GettingClaims.v1;

public static class GetClaimsEndpoint
{
    internal static RouteHandlerBuilder MapGetClaimsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/claims", GetClaims)
            .RequireAuthorization()
            .WithTags(IdentityConfigs.Tag)
            .Produces<GetClaimsResponse>()
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithOpenApi(
                operation => new(operation) { Summary = "Getting User Claims", Description = "Getting User Claims" }
            )
            .WithDisplayName("Get User claims");
    }

    private static async Task<IResult> GetClaims(IGatewayProcessor<IdentityModuleConfiguration> queryProcessor, CancellationToken cancellationToken)
    {
        return await queryProcessor.ExecuteQuery(async processor =>
        {
            var result = await processor.SendAsync(new GetClaims(), cancellationToken);

            return Results.Ok(result);
        });
    }
}
