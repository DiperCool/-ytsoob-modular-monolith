using BuildingBlocks.Monitoring;

namespace Ytsoob.Api.Extensions.ApplicationBuilderExtensions;

public static partial class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseECommerceMonitoring(this IApplicationBuilder app)
    {
        app.UseMonitoring();

        return app;
    }
}
