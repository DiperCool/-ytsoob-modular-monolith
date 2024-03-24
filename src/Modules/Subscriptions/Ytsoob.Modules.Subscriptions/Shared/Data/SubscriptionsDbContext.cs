using System.Reflection;
using BuildingBlocks.Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Subscriptions.Shared.Contracts;
using Ytsoob.Modules.Subscriptions.Subscriptions.Models;
using Ytsoob.Modules.Subscriptions.Ytsoobers.Models;

namespace Ytsoob.Modules.Subscriptions.Shared.Data;

public class SubscriptionsDbContext : EfDbContextBase, ISubscriptionsDbContext
{
    public SubscriptionsDbContext(DbContextOptions options)
        : base(options) { }

    public const string DefaultSchema = "subscriptions";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension(EfConstants.UuidGenerator);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Subscription> Subscriptions { get; }
    public DbSet<Ytsoober> Ytsoobers { get; }
}
