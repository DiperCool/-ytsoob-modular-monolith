using BuildingBlocks.Persistence.EfCore.Postgres;

namespace Ytsoob.Modules.Posts.Shared.Data;

public class DbContextDesignFactory : DbContextDesignFactoryBase<PostsDbContext>
{
    public DbContextDesignFactory()
        : base("Server=localhost;Port=5432;Database=ytsoob.services.posts;User Id=postgres;Password=postgres;") { }
}
