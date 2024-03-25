using Asp.Versioning;
using BuildingBlocks.Abstractions.Web.Module;
using BuildingBlocks.Core;
using BuildingBlocks.Core.Extensions;
using BuildingBlocks.Core.Messaging.Extensions;
using BuildingBlocks.Messaging.Persistence.Postgres.Extensions;
using BuildingBlocks.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ytsoob.Modules.Subscriptions.Shared.Extensions.ApplicationBuilderExtensions;
using Ytsoob.Modules.Subscriptions.Shared.Extensions.ServiceCollectionExtensions;

namespace Ytsoob.Modules.Subscriptions;

public class SubscriptionsModuleConfiguration : IModuleDefinition
{
    public const string ModulePrefixUri = "api/v{version:apiVersion}";
    public const string ModuleName = "Subscriptions";

    public void AddModuleServices(
        IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment
    )
    {
        services.AddInfrastructure(configuration);

        // Add Sub Modules Endpoints
    }

    public async Task ConfigureModule(
        IApplicationBuilder app,
        IConfiguration configuration,
        ILogger logger,
        IWebHostEnvironment environment
    )
    {
        ServiceActivator.Configure(app.ApplicationServices);
        if (environment.IsEnvironment("test") == false)
        {
            await app.ApplicationServices.StartHostedServices();
        }
        await app.UsePostgresPersistenceMessage<SubscriptionsModuleConfiguration>();

        await app.ApplyDatabaseMigrations(logger);
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var v1 = new ApiVersion(1, 0);

        endpoints
            .MapGet(
                "Subscriptions",
                (HttpContext context) =>
                {
                    var requestId = context.Request.Headers.TryGetValue("X-Request-Id", out var requestIdHeader)
                        ? requestIdHeader.FirstOrDefault()
                        : string.Empty;

                    return $"Subscriptions Service Apis, RequestId: {requestId}";
                }
            )
            .ExcludeFromDescription();

        // Add Sub Modules Endpoints
    }
}
