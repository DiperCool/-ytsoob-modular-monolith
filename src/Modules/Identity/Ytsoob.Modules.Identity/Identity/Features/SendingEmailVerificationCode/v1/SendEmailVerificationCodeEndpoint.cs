using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Web;
using Hellang.Middleware.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;
using Ytsoob.Modules.Identity.Users;

namespace Ytsoob.Modules.Identity.Identity.Features.SendingEmailVerificationCode.v1;

public static class SendEmailVerificationCodeEndpoint
{
  internal static IEndpointRouteBuilder MapSendEmailVerificationCodeEndpoint(
    this IEndpointRouteBuilder endpoints
  )
  {
    endpoints
      .MapPost(
        $"{IdentityConfigs.IdentityPrefixUri}/send-email-verification-code",
        SendEmailVerificationCode
      )
      .AllowAnonymous()
      .WithTags(IdentityConfigs.Tag)
      .Produces(StatusCodes.Status200OK)
      .Produces<StatusCodeProblemDetails>(StatusCodes.Status409Conflict)
      .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
      .WithName("SendEmailVerificationCode")
      .WithDisplayName("Send Email Verification Code.")
      .WithMetadata(
        new SwaggerOperationAttribute(
          "Sending Email Verification Code.",
          "Sending Email Verification Code."
        )
      )
      .WithApiVersionSet(IdentityConfigs.VersionSet)
      .HasApiVersion(1.0);
    return endpoints;
  }

  private static async Task<IResult> SendEmailVerificationCode(
    SendEmailVerificationCodeRequest request,
    [FromServices] ICommandProcessor commandProcessor,
    CancellationToken cancellationToken
  )
  {
    var command = new SendEmailVerificationCode(request.Email);

    await commandProcessor.SendAsync(command, cancellationToken);

    return Results.Ok();
  }
}
