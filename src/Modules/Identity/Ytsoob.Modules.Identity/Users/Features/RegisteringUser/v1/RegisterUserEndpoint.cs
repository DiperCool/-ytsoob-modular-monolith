using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Web;
using Hellang.Middleware.ProblemDetails;
using Swashbuckle.AspNetCore.Annotations;

namespace Ytsoob.Modules.Identity.Users.Features.RegisteringUser.v1;

public static class RegisterUserEndpoint
{
    internal static IEndpointRouteBuilder MapRegisterNewUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPost($"{UsersConfigs.UsersPrefixUri}/reg-user", RegisterUser)
            .AllowAnonymous()
            .WithTags(UsersConfigs.Tag)
            .Produces<RegisterUserResponse>(StatusCodes.Status201Created)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName("RegisterUser")
            .WithDisplayName("Register New user.")
            .WithMetadata(new SwaggerOperationAttribute("Register New User.", "Register New User."))
            .WithApiVersionSet(UsersConfigs.VersionSet)
            .HasApiVersion(1.0);
        return endpoints;
    }

    private static async Task<IResult> RegisterUser(
        RegisterUserRequest request,
        IGatewayProcessor<IdentityModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken
    )
    {
        return await gatewayProcessor.ExecuteCommand(async commandProcessor =>
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

            var result = await commandProcessor.SendAsync(command, cancellationToken);

            return Results.Created($"{UsersConfigs.UsersPrefixUri}/{result.UserIdentity?.Id}", result);
        });
    }
}
