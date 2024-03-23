using Microsoft.AspNetCore.Http;

namespace Ytsoob.Modules.Posts.Shared.Contracts;

public interface ICommentFilesBlobStorage
{
    Task<IEnumerable<string?>> UploadFilesAsync(
        IEnumerable<IFormFile> files,
        CancellationToken cancellationToken = default
    );
    Task RemoveFilesAsync(IEnumerable<string> files, CancellationToken cancellationToken = default);
}
