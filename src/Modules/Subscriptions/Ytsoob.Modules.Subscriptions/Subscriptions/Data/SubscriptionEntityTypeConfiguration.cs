using BuildingBlocks.Core.Persistence.EfCore;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ytsoob.Modules.Subscriptions.Shared.Data;
using Ytsoob.Modules.Subscriptions.Subscriptions.Models;

namespace Ytsoob.Modules.Subscriptions.Subscriptions.Data;

public class SubscriptionEntityTypeConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.ToTable(nameof(Subscription).Pluralize().Underscore(), SubscriptionsDbContext.DefaultSchema);

        // ids will use strongly typed-id value converter selector globally
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
        builder.OwnsOne(
            x => x.Description,
            a =>
            {
                a.Property(p => p.Value).HasColumnName(nameof(ValueObjects.Description).Underscore()).IsRequired();
            }
        );
        builder.OwnsOne(
            x => x.Price,
            a =>
            {
                a.Property(p => p.Value).HasColumnName(nameof(ValueObjects.Price).Underscore()).IsRequired();
            }
        );
        builder.OwnsOne(
            x => x.Title,
            a =>
            {
                a.Property(p => p.Value).HasColumnName(nameof(ValueObjects.Title).Underscore()).IsRequired();
            }
        );
        builder.Property(x => x.Created).HasDefaultValueSql(EfConstants.DateAlgorithm);
    }
}
