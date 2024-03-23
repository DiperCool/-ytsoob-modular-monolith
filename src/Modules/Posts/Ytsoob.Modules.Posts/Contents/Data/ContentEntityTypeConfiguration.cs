using BuildingBlocks.Core.Persistence.EfCore;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ytsoob.Modules.Posts.Contents.Models;
using Ytsoob.Modules.Posts.Contents.ValueObjects;
using Ytsoob.Modules.Posts.Shared.Data;

namespace Ytsoob.Modules.Posts.Contents.Data;

public class ContentEntityTypeConfiguration : IEntityTypeConfiguration<Content>
{
    public void Configure(EntityTypeBuilder<Content> builder)
    {
        builder.ToTable(nameof(Content).Pluralize().Underscore(), PostsDbContext.DefaultSchema);

        // ids will use strongly typed-id value converter selector globally
        builder.Property(x => x.Id).HasConversion(id => id.Value, id => ContentId.Of(id)).ValueGeneratedNever();
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
        builder.OwnsOne(
            x => x.ContentText,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName(nameof(Content.ContentText).Underscore())
                    .IsRequired()
                    .HasMaxLength(EfConstants.Lenght.Medium);
            }
        );
        builder.Property(x => x.Created).HasDefaultValueSql(EfConstants.DateAlgorithm);
    }
}
