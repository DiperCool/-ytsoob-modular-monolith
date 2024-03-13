using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.CQRS.Query;
using Hellang.Middleware.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;
using Ytsoob.Modules.Identity.Users.Features.RegisteringUser.v1;

namespace Ytsoob.Modules.Identity.Users.Features.GettingUserById.v1;

public static class GetUserByIdEndpoint
{
  internal static IEndpointRouteBuilder MapGetUserByIdEndpoint(this IEndpointRouteBuilder endpoints)
  {
    endpoints
      .MapGet($"{UsersConfigs.UsersPrefixUri}/{{userId:guid}}", GetUserById)
      .AllowAnonymous()
      .WithTags(UsersConfigs.Tag)
      .Produces<RegisterUserResponse>(StatusCodes.Status200OK)
      .Produces<StatusCodeProblemDetails>(StatusCodes.Status404NotFound)
      .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
      .WithName("GetUserById")
      .WithDisplayName("Get User by InternalCommandId.")
      .WithMetadata(
        new SwaggerOperationAttribute(
          "Getting User by InternalCommandId",
          "Getting User by InternalCommandId"
        )
      )
      .WithApiVersionSet(UsersConfigs.VersionSet)
      .HasApiVersion(1.0);
    return endpoints;
  }

  private static async Task<IResult> GetUserById(
    Guid userId,
    [FromServices] IQueryProcessor queryProcessor,
    CancellationToken cancellationToken
  )
  {
    var result = await queryProcessor.SendAsync(new GetUserById(userId), cancellationToken);

    return Results.Ok(result);
  }
}
