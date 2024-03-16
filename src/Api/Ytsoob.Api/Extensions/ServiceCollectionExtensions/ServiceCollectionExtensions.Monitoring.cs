using Ardalis.GuardClauses;
using BuildingBlocks.Core.Extensions;
using BuildingBlocks.Monitoring;
using BuildingBlocks.Persistence.EfCore.Postgres;
using Ytsoob.Modules.Identity;

namespace Ytsoob.Api.Extensions.ServiceCollectionExtensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddECommerceMonitoring(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        return services;
    }
}
