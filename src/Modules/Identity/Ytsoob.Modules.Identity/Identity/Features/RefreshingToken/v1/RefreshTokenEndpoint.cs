using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.CQRS.Command;
using Hellang.Middleware.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace Ytsoob.Modules.Identity.Identity.Features.RefreshingToken.v1;

public static class RefreshTokenEndpoint
{
  internal static IEndpointRouteBuilder MapRefreshTokenEndpoint(
    this IEndpointRouteBuilder endpoints
  )
  {
    endpoints
      .MapPost($"{IdentityConfigs.IdentityPrefixUri}/refresh-token", RefreshToken)
      .RequireAuthorization()
      .WithTags(IdentityConfigs.Tag)
      .Produces<RefreshTokenResponse>()
      .Produces<StatusCodeProblemDetails>(StatusCodes.Status404NotFound)
      .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
      .WithName("RefreshToken")
      .WithDisplayName("Refresh Token.")
      .WithMetadata(new SwaggerOperationAttribute("Refreshing Token", "Refreshing Token"))
      .WithApiVersionSet(IdentityConfigs.VersionSet)
      .HasApiVersion(1.0);
    return endpoints;
  }

  private static async Task<IResult> RefreshToken(
    RefreshTokenRequest request,
    [FromServices] ICommandProcessor commandProcessor,
    CancellationToken cancellationToken
  )
  {
    var command = new RefreshToken(request.AccessToken, request.RefreshToken);

    var result = await commandProcessor.SendAsync(command, cancellationToken);

    return Results.Ok(result);
  }
}
