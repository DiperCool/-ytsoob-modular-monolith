using BuildingBlocks.Core.Persistence.EfCore;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ytsoob.Modules.Ytsoobers.Profiles.Models;
using Ytsoob.Modules.Ytsoobers.Profiles.ValueObjects;
using Ytsoob.Modules.Ytsoobers.Shared.Data;

namespace Ytsoob.Modules.Ytsoobers.Profiles.Data;

public class ProfileEntityTypeConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable(nameof(Profile).Pluralize().Underscore(), YtsoobersDbContext.DefaultSchema);

        // ids will use strongly typed-id value converter selector globally
        builder.Property(x => x.Id).HasConversion(id => id.Value, id => ProfileId.Of(id)).ValueGeneratedNever();
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();

        builder.Property(x => x.Created).HasDefaultValueSql(EfConstants.DateAlgorithm);
        builder.OwnsOne(
            x => x.FirstName,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Profile.FirstName).Underscore())
                    .IsRequired()
                    .HasMaxLength(EfConstants.Lenght.Medium);
            }
        );
        builder.OwnsOne(
            x => x.LastName,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Profile.LastName).Underscore())
                    .IsRequired()
                    .HasMaxLength(EfConstants.Lenght.Medium);
            }
        );
    }
}
