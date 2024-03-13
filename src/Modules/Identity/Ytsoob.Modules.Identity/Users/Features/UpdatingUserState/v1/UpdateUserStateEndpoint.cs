using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Web;
using Hellang.Middleware.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;
using Ytsoob.Modules.Identity.Users.Features.RegisteringUser.v1;

namespace Ytsoob.Modules.Identity.Users.Features.UpdatingUserState.v1;

public static class UpdateUserStateEndpoint
{
  internal static IEndpointRouteBuilder MapUpdateUserStateEndpoint(
    this IEndpointRouteBuilder endpoints
  )
  {
    endpoints
      .MapPut($"{UsersConfigs.UsersPrefixUri}/{{userId:guid}}/state", UpdateUserState)
      .AllowAnonymous()
      .WithTags(UsersConfigs.Tag)
      .Produces<RegisterUserResponse>(StatusCodes.Status204NoContent)
      .Produces<StatusCodeProblemDetails>(StatusCodes.Status404NotFound)
      .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
      .WithName("UpdateUserState")
      .WithDisplayName("Update User State.")
      .WithMetadata(new SwaggerOperationAttribute("Updating User State.", "Updating User State"))
      .WithApiVersionSet(UsersConfigs.VersionSet)
      .HasApiVersion(1.0);
    return endpoints;
  }

  private static async Task<IResult> UpdateUserState(
    Guid userId,
    UpdateUserStateRequest request,
    IGatewayProcessor<IdentityModuleConfiguration> commandProcessor,
    CancellationToken cancellationToken
  )
  {
    return await commandProcessor.ExecuteCommand(async processor =>
    {
      var command = new UpdateUserState(userId, request.UserState);

      await processor.SendAsync(command, cancellationToken);

      return Results.NoContent();
    });
  }
}
