using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.CQRS.Query;
using Hellang.Middleware.ProblemDetails;

namespace Ytsoob.Modules.Identity.Identity.Features.GettingClaims.v1;

public static class GetClaimsEndpoint
{
  internal static IEndpointRouteBuilder MapGetClaimsEndpoint(this IEndpointRouteBuilder endpoints)
  {
    endpoints
      .MapGet($"{IdentityConfigs.IdentityPrefixUri}/claims", GetClaims)
      .RequireAuthorization()
      .WithTags(IdentityConfigs.Tag)
      .Produces<GetClaimsResponse>()
      .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
      .WithOpenApi(operation =>
        new(operation) { Summary = "Getting User Claims", Description = "Getting User Claims" }
      )
      .WithDisplayName("Get User claims")
      .WithApiVersionSet(IdentityConfigs.VersionSet)
      .HasApiVersion(1.0);
    return endpoints;
  }

  private static async Task<IResult> GetClaims(
    [FromServices] IQueryProcessor queryProcessor,
    CancellationToken cancellationToken
  )
  {
    var result = await queryProcessor.SendAsync(new GetClaims(), cancellationToken);

    return Results.Ok(result);
  }
}
