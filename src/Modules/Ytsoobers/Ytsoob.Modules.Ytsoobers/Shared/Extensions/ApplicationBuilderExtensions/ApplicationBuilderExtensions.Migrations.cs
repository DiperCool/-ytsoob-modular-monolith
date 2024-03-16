using BuildingBlocks.Persistence.EfCore.Postgres;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ytsoob.Modules.Ytsoobers.Shared.Data;

namespace Ytsoob.Modules.Ytsoobers.Shared.Extensions.ApplicationBuilderExtensions;

public static partial class ApplicationBuilderExtensions
{
    public static async Task ApplyDatabaseMigrations(this IApplicationBuilder app, ILogger logger)
    {
        var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
        if (
            !configuration.GetValue<bool>(
                $"{YtsoobersModuleConfiguration.ModuleName}:{nameof(PostgresOptions)}:UseInMemory"
            )
        )
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<YtsoobersDbContext>();

            logger.LogInformation("Updating database...");

            await dbContext.Database.MigrateAsync();

            logger.LogInformation("Updated database");
        }
    }
}
