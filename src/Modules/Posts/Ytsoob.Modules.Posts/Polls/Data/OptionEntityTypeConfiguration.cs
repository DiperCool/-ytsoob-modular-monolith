using BuildingBlocks.Core.Persistence.EfCore;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ytsoob.Modules.Posts.Polls.Models;
using Ytsoob.Modules.Posts.Polls.ValueObjects;
using Ytsoob.Modules.Posts.Shared.Data;

namespace Ytsoob.Modules.Posts.Polls.Data;

public class OptionEntityTypeConfiguration : IEntityTypeConfiguration<Option>
{
    public void Configure(EntityTypeBuilder<Option> builder)
    {
        builder.ToTable(nameof(Option).Pluralize().Underscore(), PostsDbContext.DefaultSchema);

        // ids will use strongly typed-id value converter selector globally
        builder.Property(x => x.Id).HasConversion(id => id.Value, id => OptionId.Of(id)).ValueGeneratedNever();
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
        builder.OwnsOne(
            x => x.Count,
            a =>
            {
                a.Property(p => p.Value).HasColumnName(nameof(ValueObjects.OptionCount).Underscore()).IsRequired();
            }
        );
        builder.OwnsOne(
            x => x.Fiction,
            a =>
            {
                a.Property(p => p.Value).HasColumnName(nameof(ValueObjects.Fiction).Underscore()).IsRequired();
            }
        );
        builder.OwnsOne(
            x => x.Title,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(ValueObjects.OptionTitle).Underscore())
                    .IsRequired()
                    .HasMaxLength(EfConstants.Lenght.Medium);
            }
        );
        builder.Property(x => x.Created).HasDefaultValueSql(EfConstants.DateAlgorithm);
    }
}
