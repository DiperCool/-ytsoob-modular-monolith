using BuildingBlocks.Core.Persistence.EfCore;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ytsoob.Modules.Posts.Polls.Models;
using Ytsoob.Modules.Posts.Posts.Models;
using Ytsoob.Modules.Posts.Posts.ValueObjects;
using Ytsoob.Modules.Posts.Shared.Data;

namespace Ytsoob.Modules.Posts.Posts.Data;

public class PostEntityTypeConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable(nameof(Post).Pluralize().Underscore(), PostsDbContext.DefaultSchema);

        // ids will use strongly typed-id value converter selector globally
        builder.Property(x => x.Id).HasConversion(id => id.Value, id => PostId.Of(id)).ValueGeneratedNever();
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
        builder.Property(x => x.Created).HasDefaultValueSql(EfConstants.DateAlgorithm);

        builder.HasOne(x => x.Poll).WithOne(x => x.Post).HasForeignKey<Poll>(x => x.PostId);
    }
}
