using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.CQRS.Query;
using BuildingBlocks.Abstractions.Web;
using Hellang.Middleware.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;
using Ytsoob.Modules.Identity.Users;

namespace Ytsoob.Modules.Identity.Identity.Features.RevokingRefreshToken.v1;

public static class RevokeRefreshTokenEndpoint
{
  internal static IEndpointRouteBuilder MapRevokeTokenEndpoint(this IEndpointRouteBuilder endpoints)
  {
    endpoints
      .MapPost($"{IdentityConfigs.IdentityPrefixUri}/revoke-refresh-token", RevokeToken)
      .WithTags(IdentityConfigs.Tag)
      .RequireAuthorization()
      .Produces<StatusCodeProblemDetails>(StatusCodes.Status204NoContent)
      .Produces<StatusCodeProblemDetails>(StatusCodes.Status404NotFound)
      .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
      .WithDisplayName("Revoke refresh token.")
      .WithMetadata(
        new SwaggerOperationAttribute("Revoking Refresh Token", "Revoking Refresh Token")
      )
      .WithApiVersionSet(IdentityConfigs.VersionSet)
      .HasApiVersion(1.0);

    return endpoints;
  }

  private static async Task<IResult> RevokeToken(
    RevokeRefreshTokenRequest request,
    [FromServices] ICommandProcessor commandProcessor,
    CancellationToken cancellationToken
  )
  {
    var command = new RevokeRefreshToken(request.RefreshToken);

    await commandProcessor.SendAsync(command, cancellationToken);

    return Results.NoContent();
  }
}
