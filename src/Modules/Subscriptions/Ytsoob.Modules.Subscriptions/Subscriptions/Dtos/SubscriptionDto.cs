namespace Ytsoob.Modules.Subscriptions.Subscriptions.Dtos;

public class SubscriptionDto
{
    public long Id { get; set; }
    public string Title { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public string? Photo { get; private set; }
    public decimal Price { get; private set; }
}
