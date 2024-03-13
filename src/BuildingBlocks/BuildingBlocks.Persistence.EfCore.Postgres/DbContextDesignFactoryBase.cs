using BuildingBlocks.Core.Extensions;
using BuildingBlocks.Core.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BuildingBlocks.Persistence.EfCore.Postgres;

public abstract class DbContextDesignFactoryBase<TDbContext> : IDesignTimeDbContextFactory<TDbContext>
    where TDbContext : DbContext
{
    private readonly string _connectionString;

    protected DbContextDesignFactoryBase(string connectionString)
    {
        _connectionString = connectionString;
    }

    public TDbContext CreateDbContext(string[] args)
    {
        Console.WriteLine($"BaseDirectory: {AppContext.BaseDirectory}");
        Console.WriteLine($"Postgres Connection String: {_connectionString}");


        var optionsBuilder = new DbContextOptionsBuilder<TDbContext>()
            .UseNpgsql(
                _connectionString,
                sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(GetType().Assembly.FullName);
                    sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null);
                }
            ).UseSnakeCaseNamingConvention();

        Console.WriteLine(_connectionString);
        return (TDbContext)Activator.CreateInstance(typeof(TDbContext), optionsBuilder.Options);
    }
}
