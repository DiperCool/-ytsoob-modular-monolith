using Asp.Versioning.Builder;
using Ytsoob.Modules.Identity.Users.Features.GettingUerByEmail.v1;
using Ytsoob.Modules.Identity.Users.Features.GettingUserById.v1;
using Ytsoob.Modules.Identity.Users.Features.RegisteringUser.v1;
using Ytsoob.Modules.Identity.Users.Features.UpdatingUserState.v1;

namespace Ytsoob.Modules.Identity.Users;

internal static class UsersConfigs
{
    public const string Tag = "Users";
    public const string UsersPrefixUri = $"{IdentityModuleConfiguration.IdentityModulePrefixUri}/users";
    public static ApiVersionSet VersionSet { get; private set; } = default!;

    internal static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder endpoints)
    {
        VersionSet = endpoints.NewApiVersionSet(Tag).Build();

        // create a new sub group for each version


        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0#route-groups
        // https://github.com/dotnet/aspnet-api-versioning/blob/main/examples/AspNetCore/WebApi/MinimalOpenApiExample/Program.cs
        endpoints.MapRegisterNewUserEndpoint();
        endpoints.MapUpdateUserStateEndpoint();
        endpoints.MapGetUserByIdEndpoint();
        endpoints.MapGetUserByEmailEndpoint();

        return endpoints;
    }
}
