using BuildingBlocks.Core.Domain;
using Ytsoob.Modules.Ytsoobers.Profiles.ValueObjects;

namespace Ytsoob.Modules.Ytsoobers.Profiles.Models;

public class Profile : Entity<ProfileId>
{
    public FirstName? FirstName { get; private set; }
    public LastName? LastName { get; private set; }
    public string? Avatar { get; private set; }

    // ef core
    protected Profile() { }

    protected Profile(ProfileId profileId, FirstName firstName, LastName lastName)
    {
        Id = profileId;
        FirstName = firstName;
        LastName = lastName;
    }

    protected Profile(ProfileId profileId) => Id = profileId;

    public static Profile Of(ProfileId profileId, FirstName firstName, LastName lastName) =>
        new(profileId, firstName, lastName);

    public static Profile Of(ProfileId profileId) => new(profileId);

    public void Update(FirstName firstName, LastName lastName, string? avatar)
    {
        FirstName = firstName;
        LastName = lastName;
        Avatar = avatar;
    }
}
