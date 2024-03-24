namespace Ytsoob.Modules.Subscriptions.Ytsoobers.Models;

public class Ytsoober
{
    public long Id { get; set; }
    public Guid IdentityId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Username { get; set; }
    public string? Avatar { get; set; }
}
