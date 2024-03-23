using Asp.Versioning;
using BuildingBlocks.Abstractions.Web.Module;
using BuildingBlocks.Core;
using BuildingBlocks.Core.Extensions;
using BuildingBlocks.Core.Messaging.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ytsoob.Modules.Ytsoobers.Profiles;
using Ytsoob.Modules.Ytsoobers.Shared.Extensions.ApplicationBuilderExtensions;
using Ytsoob.Modules.Ytsoobers.Shared.Extensions.ServiceCollectionExtensions;

namespace Ytsoob.Modules.Ytsoobers;

public class YtsoobersModuleConfiguration : IModuleDefinition
{
    public const string IdentityModulePrefixUri = "api/v{version:apiVersion}/ytsoobers";
    public const string ModuleName = "Ytsoobers";

    public void AddModuleServices(
        IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment
    )
    {
        services.AddInfrastructure(configuration);
        services.AddProfilesServices(configuration);
        // Add Sub Modules Endpoints
    }

    public async Task ConfigureModule(
        IApplicationBuilder app,
        IConfiguration configuration,
        ILogger logger,
        IWebHostEnvironment environment
    )
    {
        if (environment.IsEnvironment("test") == false)
        {
            await app.ApplicationServices.StartHostedServices();
        }

        ServiceActivator.Configure(app.ApplicationServices);

        app.SubscribeAllMessageFromAssemblyOfType<YtsoobersRoot>();

        await app.ApplyDatabaseMigrations(logger);
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var v1 = new ApiVersion(1, 0);

        endpoints.MapProfilesEndpoints();

        endpoints
            .MapGet(
                "Ytsoobers",
                (HttpContext context) =>
                {
                    var requestId = context.Request.Headers.TryGetValue("X-Request-Id", out var requestIdHeader)
                        ? requestIdHeader.FirstOrDefault()
                        : string.Empty;

                    return $"Ytsoobers Service Apis, RequestId: {requestId}";
                }
            )
            .ExcludeFromDescription();

        // Add Sub Modules Endpoints
    }
}
