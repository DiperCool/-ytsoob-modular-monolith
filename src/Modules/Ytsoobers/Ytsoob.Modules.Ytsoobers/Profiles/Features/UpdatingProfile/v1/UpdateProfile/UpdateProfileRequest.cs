using Microsoft.AspNetCore.Http;

namespace Ytsoob.Modules.Ytsoobers.Profiles.Features.UpdatingProfile.v1.UpdateProfile;

public record UpdateProfileRequest(string FirstName, string LastName, IFormFile? Avatar);
