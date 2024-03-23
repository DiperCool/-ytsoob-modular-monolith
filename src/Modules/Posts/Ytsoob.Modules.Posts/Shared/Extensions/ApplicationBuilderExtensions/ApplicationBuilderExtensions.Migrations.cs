using BuildingBlocks.Persistence.EfCore.Postgres;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ytsoob.Modules.Posts.Shared.Data;

namespace Ytsoob.Modules.Posts.Shared.Extensions.ApplicationBuilderExtensions;

public static partial class ApplicationBuilderExtensions
{
    public static async Task ApplyDatabaseMigrations(this IApplicationBuilder app, ILogger logger)
    {
        var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
        if (
            !configuration.GetValue<bool>(
                $"{PostsModuleConfiguration.ModuleName}:{nameof(PostgresOptions)}:UseInMemory"
            )
        )
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<PostsDbContext>();

            logger.LogInformation("Updating database...");

            await dbContext.Database.MigrateAsync();

            logger.LogInformation("Updated database");
        }
    }
}
