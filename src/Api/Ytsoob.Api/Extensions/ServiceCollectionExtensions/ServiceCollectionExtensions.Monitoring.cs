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
        IConfiguration configuration)
    {
        services.AddMonitoring(healthChecksBuilder =>
        {
            var identityPostgresOptions = configuration.GetOptions<PostgresOptions>(
                $"{IdentityModuleConfiguration.ModuleName}:{nameof(PostgresOptions)}");
            Guard.Against.Null(identityPostgresOptions, nameof(identityPostgresOptions));

            healthChecksBuilder.AddNpgSql(
                identityPostgresOptions.ConnectionString,
                name: "Identity-Module-Postgres-Check",
                tags: new[] {"identity-postgres"});
        });

        return services;
    }
}
