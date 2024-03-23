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
using Ytsoob.Modules.Posts.Comments;
using Ytsoob.Modules.Posts.Contents;
using Ytsoob.Modules.Posts.Polls;
using Ytsoob.Modules.Posts.Posts;
using Ytsoob.Modules.Posts.Shared.Extensions.ApplicationBuilderExtensions;
using Ytsoob.Modules.Posts.Shared.Extensions.ServiceCollectionExtensions;

namespace Ytsoob.Modules.Posts;

public class PostsModuleConfiguration : IModuleDefinition
{
    public const string PostsModulePrefixUri = "api/v{version:apiVersion}/posts";
    public const string ModuleName = "Posts";

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
        if (environment.IsEnvironment("test") == false)
        {
            await app.ApplicationServices.StartHostedServices();
        }

        ServiceActivator.Configure(app.ApplicationServices);

        app.SubscribeAllMessageFromAssemblyOfType<PostsRoot>();

        await app.ApplyDatabaseMigrations(logger);
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var v1 = new ApiVersion(1, 0);
        endpoints.MapPostsEndpoints();
        endpoints.MapContentsEndpoints();
        endpoints.MapPollEndpoints();
        endpoints.MapCommentsEndpoints();
        endpoints
            .MapGet(
                "Posts",
                (HttpContext context) =>
                {
                    var requestId = context.Request.Headers.TryGetValue("X-Request-Id", out var requestIdHeader)
                        ? requestIdHeader.FirstOrDefault()
                        : string.Empty;

                    return $"Posts Service Apis, RequestId: {requestId}";
                }
            )
            .ExcludeFromDescription();

        // Add Sub Modules Endpoints
    }
}
