using BuildingBlocks.Core.Persistence.EfCore;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ytsoob.Modules.Ytsoobers.Shared.Data;
using Ytsoob.Modules.Ytsoobers.Ytsoobers.Models;
using Ytsoob.Modules.Ytsoobers.Ytsoobers.ValueObjects;

namespace Ytsoob.Modules.Ytsoobers.Ytsoobers.Data;

public class PostEntityTypeConfiguration : IEntityTypeConfiguration<Ytsoober>
{
    public void Configure(EntityTypeBuilder<Ytsoober> builder)
    {
        builder.ToTable(nameof(Ytsoober).Pluralize().Underscore(), YtsoobersDbContext.DefaultSchema);

        // ids will use strongly typed-id value converter selector globally
        builder.Property(x => x.Id).HasConversion(id => id.Value, id => YtsooberId.Of(id)).ValueGeneratedNever();
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
        builder.Property(x => x.Created).HasDefaultValueSql(EfConstants.DateAlgorithm);
        builder.OwnsOne(
            x => x.Email,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Ytsoober.Email).Underscore())
                    .IsRequired()
                    .HasMaxLength(EfConstants.Lenght.Medium);
            }
        );

        builder.OwnsOne(
            x => x.Username,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Ytsoober.Username).Underscore())
                    .IsRequired()
                    .HasMaxLength(EfConstants.Lenght.Medium);
            }
        );
    }
}
