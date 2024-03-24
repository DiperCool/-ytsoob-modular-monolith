using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Ytsoob.Modules.Subscriptions.Subscriptions;

internal static class SubscriptionsConfigs
{
    public const string Tag = "Subscriptions";
    public const string PrefixUri = $"{SubscriptionsModuleConfiguration.ModulePrefixUri}/subscriptions";
    public static ApiVersionSet VersionSet { get; private set; } = default!;

    internal static IEndpointRouteBuilder MapSubscriptionsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        VersionSet = endpoints.NewApiVersionSet(Tag).Build();

        // create a new sub group for each version


        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0#route-groups
        // https://github.com/dotnet/aspnet-api-versioning/blob/main/examples/AspNetCore/WebApi/MinimalOpenApiExample/Program.cs


        return endpoints;
    }
}
