using BuildingBlocks.Abstractions.Web;
using BuildingBlocks.Abstractions.Web.Module;
using BuildingBlocks.Messaging.Persistence.Postgres.MessagePersistence;
using BuildingBlocks.Web.Module;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Messaging.Persistence.Postgres.Extensions;

public static class ApplicationBuilderExtensions
{
    public static async Task UsePostgresPersistenceMessage<T>(this IApplicationBuilder app)
        where T : class, IModuleDefinition
    {
        using var serviceScope = CompositionRootRegistry.GetByModule<T>()?.ServiceProvider.CreateScope();
        var messagePersistenceDbContext =
            serviceScope!.ServiceProvider.GetRequiredService<MessagePersistenceDbContext>();

        await messagePersistenceDbContext.Database.MigrateAsync();
    }
}
