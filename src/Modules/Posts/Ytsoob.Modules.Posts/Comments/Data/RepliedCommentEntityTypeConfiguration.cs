using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ytsoob.Modules.Posts.Comments.Models;

namespace Ytsoob.Modules.Posts.Comments.Data;

public class RepliedCommentEntityTypeConfiguration : IEntityTypeConfiguration<RepliedComment>
{
    public void Configure(EntityTypeBuilder<RepliedComment> builder)
    {
        builder
            .HasOne(x => x.Comment)
            .WithMany(x => x.RepliedComments)
            .HasForeignKey(x => x.CommentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
