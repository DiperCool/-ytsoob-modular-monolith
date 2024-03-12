using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Web;
using Hellang.Middleware.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace Ytsoob.Modules.Identity.Users.Features.RegisteringUser.v1;

public static class RegisterUserEndpoint
{
    internal static RouteHandlerBuilder MapRegisterNewUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", RegisterUser)
            .AllowAnonymous()
            .WithTags(UsersConfigs.Tag)
            .Produces<RegisterUserResponse>(StatusCodes.Status201Created)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName("RegisterUser")
            .WithDisplayName("Register New user.")
            .WithMetadata(new SwaggerOperationAttribute("Register New User.", "Register New User."));
    }

    private static async Task<IResult> RegisterUser(
        RegisterUserRequest request,
        IGatewayProcessor<IdentityModuleConfiguration> commandProcessor,
        CancellationToken cancellationToken
    )
    {
        return await commandProcessor.ExecuteCommand(async processor =>
        {
            var command = new RegisterUser(
                request.FirstName,
                request.LastName,
                request.UserName,
                request.Email,
                request.Password,
                request.Phone,
                request.ConfirmPassword,
                request.Roles?.ToList()
            );

            var result = await processor.SendAsync(command, cancellationToken);

            return Results.Created($"{UsersConfigs.UsersPrefixUri}/{result.UserIdentity?.Id}", result);
        });
    }
}
