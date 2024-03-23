using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ytsoob.Modules.Posts.Reactions.Models;
using Ytsoob.Modules.Posts.Shared.Data;

namespace Ytsoob.Modules.Posts.Reactions.Data;

public class ReactionStatsEntityTypeConfiguration : IEntityTypeConfiguration<ReactionStats>
{
    public void Configure(EntityTypeBuilder<ReactionStats> builder)
    {
        builder.ToTable(nameof(ReactionStats).Pluralize().Underscore(), PostsDbContext.DefaultSchema);

        // ids will use strongly typed-id value converter selector globally
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
        builder.OwnsOne(
            x => x.Angry,
            a =>
            {
                a.Property(p => p.Value).HasColumnName(nameof(ReactionStats.Angry).Underscore()).IsRequired();
            }
        );
        builder.OwnsOne(
            x => x.Crying,
            a =>
            {
                a.Property(p => p.Value).HasColumnName(nameof(ReactionStats.Crying).Underscore()).IsRequired();
            }
        );
        builder.OwnsOne(
            x => x.Dislike,
            a =>
            {
                a.Property(p => p.Value).HasColumnName(nameof(ReactionStats.Dislike).Underscore()).IsRequired();
            }
        );
        builder.OwnsOne(
            x => x.Happy,
            a =>
            {
                a.Property(p => p.Value).HasColumnName(nameof(ReactionStats.Dislike).Underscore()).IsRequired();
            }
        );
        builder.OwnsOne(
            x => x.Like,
            a =>
            {
                a.Property(p => p.Value).HasColumnName(nameof(ReactionStats.Like).Underscore()).IsRequired();
            }
        );
        builder.OwnsOne(
            x => x.Wonder,
            a =>
            {
                a.Property(p => p.Value).HasColumnName(nameof(ReactionStats.Wonder).Underscore()).IsRequired();
            }
        );
    }
}
