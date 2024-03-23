using BuildingBlocks.Core.Domain;
using Ytsoob.Modules.Posts.Ytsoobers.Models;

namespace Ytsoob.Modules.Posts.Subscriptions.Models;

public class Subscription : Entity<long>
{
    public Subscription(long id, string title, string description, string? photo, decimal price, long ytsooberId)
    {
        Id = id;
        Title = title;
        Description = description;
        Photo = photo;
        Price = price;
        YtsooberId = ytsooberId;
    }

    public string Title { get; set; }
    public string Description { get; set; }
    public string? Photo { get; set; }
    public decimal Price { get; set; }
    public Ytsoober Ytsoober { get; set; } = default!;
    public long YtsooberId { get; set; }
}
