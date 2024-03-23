using Asp.Versioning.Builder;
using BuildingBlocks.BlobStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Ytsoob.Modules.Ytsoobers.Profiles.Features.GettingProfile.v1.GetProfile;
using Ytsoob.Modules.Ytsoobers.Profiles.Features.UpdatingProfile.v1.UpdateProfile;
using Ytsoob.Modules.Ytsoobers.Shared.Contracts;
using Ytsoob.Modules.Ytsoobers.Shared.Services;

namespace Ytsoob.Modules.Ytsoobers.Profiles;

internal static class ProfilesConfig
{
    public const string Tag = "Profiles";
    public const string ProfilesPrefixUri = $"{YtsoobersModuleConfiguration.IdentityModulePrefixUri}";
    public static ApiVersionSet VersionSet { get; private set; } = default!;

    public static IServiceCollection AddProfilesServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddBlobStorage(configuration, YtsoobersModuleConfiguration.ModuleName);
        services.AddSingleton<IAvatarStorage, AvatarStorage>();
        return services;
    }

    public static IEndpointRouteBuilder MapProfilesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        VersionSet = endpoints.NewApiVersionSet(Tag).Build();

        // create a new sub group for each version


        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0#route-groups
        // https://github.com/dotnet/aspnet-api-versioning/blob/main/examples/AspNetCore/WebApi/MinimalOpenApiExample/Program.cs
        endpoints.MapGetProfileEndpoint();
        endpoints.MapUpdateProfileEndpoint();
        return endpoints;
    }
}
