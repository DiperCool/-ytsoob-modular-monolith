using BuildingBlocks.Core.Persistence.EfCore;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ytsoob.Modules.Posts.Polls.ValueObjects;
using Ytsoob.Modules.Posts.Shared.Data;

namespace Ytsoob.Modules.Posts.Polls.Data;

public class PollEntityTypeConfiguration : IEntityTypeConfiguration<Models.Poll>
{
    public void Configure(EntityTypeBuilder<Models.Poll> builder)
    {
        builder.ToTable(nameof(Polls).Pluralize().Underscore(), PostsDbContext.DefaultSchema);

        // ids will use strongly typed-id value converter selector globally
        builder.Property(x => x.Id).HasConversion(id => id.Value, id => PollId.Of(id)).ValueGeneratedNever();
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
        builder.HasOne(x => x.Post).WithOne(x => x.Poll);
        builder.OwnsOne(
            x => x.TotalCountPoll,
            a =>
            {
                a.Property(p => p.Value).HasColumnName(nameof(ValueObjects.TotalCountPoll).Underscore()).IsRequired();
            }
        );
        builder.OwnsOne(
            x => x.Question,
            a =>
            {
                a.Property(p => p.Value).HasColumnName(nameof(ValueObjects.Question).Underscore()).IsRequired();
            }
        );
        builder.Property(x => x.Created).HasDefaultValueSql(EfConstants.DateAlgorithm);
        builder.Navigation(x => x.Options).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
