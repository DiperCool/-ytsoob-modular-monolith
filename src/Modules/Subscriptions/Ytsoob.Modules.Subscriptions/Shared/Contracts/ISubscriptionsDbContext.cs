using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Subscriptions.Subscriptions.Models;
using Ytsoob.Modules.Subscriptions.Ytsoobers.Models;

namespace Ytsoob.Modules.Subscriptions.Shared.Contracts;

public interface ISubscriptionsDbContext
{
    DbSet<TEntity> Set<TEntity>()
        where TEntity : class;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DbSet<Subscription> Subscriptions { get; }
    DbSet<Ytsoober> Ytsoobers { get; }
}
