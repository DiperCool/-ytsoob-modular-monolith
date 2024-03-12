using BuildingBlocks.Persistence.EfCore.Postgres;

namespace Ytsoob.Modules.Identity.Shared.Data;

public class DbContextDesignFactory : DbContextDesignFactoryBase<IdentityContext>
{
    public DbContextDesignFactory() : base("Identity:PostgresOptions")
    {
    }
}
