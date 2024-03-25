using System.Reflection;
using BuildingBlocks.Core.Persistence.EfCore;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Posts.Comments.Models;
using Ytsoob.Modules.Posts.Contents.Models;
using Ytsoob.Modules.Posts.Polls.Models;
using Ytsoob.Modules.Posts.Posts.Models;
using Ytsoob.Modules.Posts.Reactions.Models;
using Ytsoob.Modules.Posts.Subscriptions.Models;
using Ytsoob.Modules.Posts.Ytsoobers.Models;

namespace Ytsoob.Modules.Posts.Shared.Data;

public class PostsDbContext : EfDbContextBase
{
    public PostsDbContext(DbContextOptions<PostsDbContext> options)
        : base(options) { }

    public const string DefaultSchema = "posts";
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Ytsoober> Ytsoobers => Set<Ytsoober>();
    public DbSet<Content> Contents => Set<Content>();
    public DbSet<Poll> Polls => Set<Poll>();
    public DbSet<Option> Options => Set<Option>();
    public DbSet<Voter> Voters => Set<Voter>();
    public DbSet<Reaction> Reactions => Set<Reaction>();
    public DbSet<BaseComment> BaseComments => Set<BaseComment>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<RepliedComment> RepliedComments => Set<RepliedComment>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension(EfConstants.UuidGenerator);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
