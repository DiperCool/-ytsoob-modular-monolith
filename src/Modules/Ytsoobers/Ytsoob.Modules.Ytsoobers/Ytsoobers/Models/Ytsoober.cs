using BuildingBlocks.Core.Domain;
using BuildingBlocks.Core.Domain.ValueObjects;
using BuildingBlocks.Core.IdsGenerator;
using Ytsoob.Modules.Ytsoobers.Profiles.Dtos.v1;
using Ytsoob.Modules.Ytsoobers.Profiles.Features.UpdatingProfile.v1.UpdateProfile;
using Ytsoob.Modules.Ytsoobers.Profiles.ValueObjects;
using Ytsoob.Modules.Ytsoobers.Ytsoobers.Features.CreatingYtsoober.v1.CreateYtsoober;
using Ytsoob.Modules.Ytsoobers.Ytsoobers.ValueObjects;

namespace Ytsoob.Modules.Ytsoobers.Ytsoobers.Models;

public class Ytsoober : Aggregate<YtsooberId>
{
    public Email Email { get; private set; } = default!;
    public Username Username { get; private set; }
    public Profiles.Models.Profile Profile { get; private set; }
    public bool CreatingCompleted { get; private set; } = default!;
    public Guid IdentityId { get; set; }

    // ef core
    protected Ytsoober() { }

    protected Ytsoober(YtsooberId id, Username username, Email email, Guid identityId)
    {
        Id = id;
        Email = email;
        IdentityId = identityId;
        Username = username;
        Profile = Profiles.Models.Profile.Of(ProfileId.Of(SnowFlakIdGenerator.NewId()));
    }

    public static Ytsoober Create(YtsooberId id, Username username, Email email, Guid identityId)
    {
        Ytsoober ytsoober = new Ytsoober(id, username, email, identityId);
        ytsoober.AddDomainEvents(new YtsooberCreated(id, identityId, username, email, new ProfileDto()));
        return ytsoober;
    }

    public void UpdateProfile(FirstName firstName, LastName lastName, string? avatar)
    {
        Profile.Update(firstName, lastName, avatar);
        AddDomainEvents(new YtsooberProfileUpdated(Id, firstName, lastName, avatar));
    }
}
