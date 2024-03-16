using BuildingBlocks.Persistence.EfCore.Postgres;

namespace Ytsoob.Modules.Ytsoobers.Shared.Data;

public class DbContextDesignFactory : DbContextDesignFactoryBase<YtsoobersDbContext>
{
  public DbContextDesignFactory()
    : base(
      "Server=localhost;Port=5432;Database=ytsoob.services.ytsoobers;User Id=postgres;Password=postgres;"
    ) { }
}
