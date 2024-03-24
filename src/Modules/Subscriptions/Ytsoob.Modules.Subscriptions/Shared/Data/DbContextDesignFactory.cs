using BuildingBlocks.Persistence.EfCore.Postgres;

namespace Ytsoob.Modules.Subscriptions.Shared.Data;

public class DbContextDesignFactory : DbContextDesignFactoryBase<SubscriptionsDbContext>
{
    public DbContextDesignFactory()
        : base("Server=localhost;Port=5432;Database=ytsoob.services.subscriptions;User Id=postgres;Password=postgres;")
    { }
}
