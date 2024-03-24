using Microsoft.AspNetCore.Http;

namespace Ytsoob.Modules.Subscriptions.Subscriptions.Contracts;

public interface ISubBlobStorage
{
    Task<string?> UploadFileAsync(IFormFile file, CancellationToken cancellationToken = default);
    Task RemoveFileAsync(string file, CancellationToken cancellationToken = default);
}
