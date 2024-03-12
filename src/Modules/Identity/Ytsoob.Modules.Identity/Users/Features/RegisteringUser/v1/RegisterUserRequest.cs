namespace Ytsoob.Modules.Identity.Users.Features.RegisteringUser.v1;

public record RegisterUserRequest(
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Phone,
    string Password,
    string ConfirmPassword
)
{
    public IEnumerable<string> Roles { get; init; } = new List<string> { IdentityConstants.Role.User };
}
