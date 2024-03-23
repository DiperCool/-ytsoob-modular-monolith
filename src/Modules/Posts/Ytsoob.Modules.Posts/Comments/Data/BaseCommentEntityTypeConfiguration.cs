using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ytsoob.Modules.Posts.Comments.Models;
using Ytsoob.Modules.Posts.Comments.ValueObjects;
using Ytsoob.Modules.Posts.Shared.Data;

namespace Ytsoob.Modules.Posts.Comments.Data;

public class BaseCommentEntityConfiguration : IEntityTypeConfiguration<BaseComment>
{
    public const string TableName = "comments";

    public virtual void Configure(EntityTypeBuilder<BaseComment> builder)
    {
        builder.ToTable(TableName, PostsDbContext.DefaultSchema);

        builder.Property(x => x.Id).HasConversion(id => id.Value, id => CommentId.Of(id)).ValueGeneratedNever();
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id).IsUnique();
        builder.OwnsOne(
            x => x.Content,
            a =>
            {
                a.Property(p => p.Value).HasColumnName(nameof(ValueObjects.CommentContent).Underscore()).IsRequired();
            }
        );
    }
}
