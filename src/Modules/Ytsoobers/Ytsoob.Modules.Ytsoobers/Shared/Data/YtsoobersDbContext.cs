using System.Reflection;
using BuildingBlocks.Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Ytsoobers.Profiles.Models;
using Ytsoob.Modules.Ytsoobers.Ytsoobers.Models;

namespace Ytsoob.Modules.Ytsoobers.Shared.Data;

public class YtsoobersDbContext : EfDbContextBase
{
    public YtsoobersDbContext(DbContextOptions options)
        : base(options) { }

    public DbSet<Ytsoober> Ytsoobers => Set<Ytsoober>();
    public DbSet<Profile> Profiles => Set<Profile>();
    public const string DefaultSchema = "ytsoobers";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension(EfConstants.UuidGenerator);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
