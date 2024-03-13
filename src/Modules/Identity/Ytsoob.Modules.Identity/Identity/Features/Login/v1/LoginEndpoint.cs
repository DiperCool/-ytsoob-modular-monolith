using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Web;
using Hellang.Middleware.ProblemDetails;
using Ytsoob.Modules.Identity.Users;

namespace Ytsoob.Modules.Identity.Identity.Features.Login.v1;

public static class LoginEndpoint
{
  internal static IEndpointRouteBuilder MapLoginUserEndpoint(this IEndpointRouteBuilder endpoints)
  {
    // https://github.com/dotnet/aspnetcore/issues/45082
    // https://github.com/dotnet/aspnetcore/issues/40753
    // https://github.com/domaindrivendev/Swashbuckle.AspNetCore/pull/2414
    endpoints
      .MapPost($"{IdentityConfigs.IdentityPrefixUri}/login", LoginUser)
      .WithTags(IdentityConfigs.Tag)
      .AllowAnonymous()
      .Produces<LoginResponse>()
      .Produces<StatusCodeProblemDetails>(StatusCodes.Status404NotFound)
      .Produces<StatusCodeProblemDetails>(StatusCodes.Status500InternalServerError)
      .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
      .WithOpenApi(operation =>
        new(operation) { Summary = "Login User", Description = "Login User" }
      )
      .WithDisplayName("Login User.")
      .WithName("Login")
      .WithApiVersionSet(IdentityConfigs.VersionSet)
      .HasApiVersion(1.0);
    return endpoints;
  }

  private static async Task<IResult> LoginUser(
    LoginRequest request,
    [FromServices] ICommandProcessor commandProcessor,
    CancellationToken cancellationToken
  )
  {
    var command = new Login(request.UserNameOrEmail, request.Password, request.Remember);

    var result = await commandProcessor.SendAsync(command, cancellationToken);

    return Results.Ok(result);
  }
}
