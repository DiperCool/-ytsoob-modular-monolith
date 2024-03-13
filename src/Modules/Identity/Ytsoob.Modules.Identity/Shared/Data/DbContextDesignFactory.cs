using BuildingBlocks.Persistence.EfCore.Postgres;

namespace Ytsoob.Modules.Identity.Shared.Data;

public class DbContextDesignFactory : DbContextDesignFactoryBase<IdentityContext>
{
    public DbContextDesignFactory() : base("Server=localhost;Port=5432;Database=ytsoob.services.identity;User Id=postgres;Password=postgres;")
    {
    }
}
