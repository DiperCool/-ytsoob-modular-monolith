using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.CQRS.Command;
using Hellang.Middleware.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace Ytsoob.Modules.Identity.Identity.Features.VerifyingEmail.v1;

public static class VerifyEmailEndpoint
{
  internal static IEndpointRouteBuilder MapSendVerifyEmailEndpoint(
    this IEndpointRouteBuilder endpoints
  )
  {
    endpoints
      .MapPost($"{IdentityConfigs.IdentityPrefixUri}/verify-email", VerifyEmail)
      .AllowAnonymous()
      .WithTags(IdentityConfigs.Tag)
      .Produces(StatusCodes.Status200OK)
      .Produces<StatusCodeProblemDetails>(StatusCodes.Status409Conflict)
      .Produces<StatusCodeProblemDetails>(StatusCodes.Status500InternalServerError)
      .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
      .WithName("VerifyEmail")
      .WithDisplayName("Verify Email.")
      .WithMetadata(new SwaggerOperationAttribute("Verifying Email", "Verifying Email"))
      .WithApiVersionSet(IdentityConfigs.VersionSet)
      .HasApiVersion(1.0);
    return endpoints;
  }

  private static async Task<IResult> VerifyEmail(
    VerifyEmailRequest request,
    [FromServices] ICommandProcessor commandProcessor,
    CancellationToken cancellationToken
  )
  {
    var command = new VerifyEmail(request.Email, request.Code);

    await commandProcessor.SendAsync(command, cancellationToken);

    return Results.Ok();
  }
}
