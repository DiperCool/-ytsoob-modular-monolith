using Microsoft.AspNetCore.Http;

namespace Ytsoob.Modules.Ytsoobers.Shared.Contracts;

public interface IAvatarStorage
{
    public Task<string?> UploadAvatarAsync(IFormFile file, CancellationToken cancellationToken);
}
