using Asp.Versioning.Builder;
using BuildingBlocks.Abstractions.Persistence;
using Ytsoob.Modules.Identity.Identity.Features.GettingClaims.v1;
using Ytsoob.Modules.Identity.Identity.Features.Login.v1;
using Ytsoob.Modules.Identity.Identity.Features.RefreshingToken.v1;
using Ytsoob.Modules.Identity.Identity.Features.RevokingRefreshToken.v1;
using Ytsoob.Modules.Identity.Identity.Features.SendingEmailVerificationCode.v1;
using Ytsoob.Modules.Identity.Identity.Features.VerifyingEmail.v1;
using Ytsoob.Modules.Identity.Identity.Data;
using Ytsoob.Modules.Identity.Shared.Extensions.ServiceCollectionExtensions;

namespace Ytsoob.Modules.Identity.Identity;

internal static class IdentityConfigs
{
    public const string Tag = "Identity";
    public const string IdentityPrefixUri = $"{IdentityModuleConfiguration.IdentityModulePrefixUri}";
    public static ApiVersionSet VersionSet { get; private set; } = default!;

    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCustomIdentity(configuration);
        services.AddCustomIdentityServer();
        services.AddScoped<IDataSeeder, IdentityDataSeeder>();


        return services;
    }


    public static IEndpointRouteBuilder MapIdentityEndpoints(this IEndpointRouteBuilder endpoints)
    {
        VersionSet = endpoints.NewApiVersionSet(Tag).Build();

        // create a new sub group for each version


        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0#route-groups
        // https://github.com/dotnet/aspnet-api-versioning/blob/main/examples/AspNetCore/WebApi/MinimalOpenApiExample/Program.cs


        endpoints.MapLoginUserEndpoint();
        endpoints.MapSendEmailVerificationCodeEndpoint();
        endpoints.MapSendVerifyEmailEndpoint();
        endpoints.MapRefreshTokenEndpoint();
        endpoints.MapRevokeTokenEndpoint();
        endpoints.MapGetClaimsEndpoint();

        return endpoints;
    }
}
