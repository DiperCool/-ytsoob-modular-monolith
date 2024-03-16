using Ytsoob.Modules.Ytsoobers.Profiles.Dtos.v1;
using Profile = AutoMapper.Profile;

namespace Ytsoob.Modules.Ytsoobers.Profiles;

public class ProfileMapper : Profile
{
    public ProfileMapper()
    {
        CreateMap<Models.Profile, ProfileDto>()
            .ForMember(x => x.LastName, opt => opt.MapFrom(x => x.LastName.Value))
            .ForMember(x => x.FirstName, opt => opt.MapFrom(x => x.FirstName.Value));
    }
}
