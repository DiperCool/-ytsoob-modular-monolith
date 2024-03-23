using BuildingBlocks.BlobStorage;
using BuildingBlocks.BlobStorage.Policies;
using Microsoft.AspNetCore.Http;
using Ytsoob.Modules.Posts.Shared.Contracts;

namespace Ytsoob.Modules.Posts.Shared.Services;

public class ContentBlobStorage : IContentBlobStorage
{
    private IMinioService _minioService;
    private const string Bucket = "posts";

    public ContentBlobStorage(IMinioService minioService)
    {
        _minioService = minioService;
    }

    public async Task<IEnumerable<string?>> UploadFilesAsync(
        IEnumerable<IFormFile> files,
        CancellationToken cancellationToken = default
    )
    {
        await _minioService.CreateBucketIfNotExistsAsync(Bucket, new ReadonlyBucketPolicy(), cancellationToken);
        return await _minioService.AddItemsAsync(Bucket, files, cancellationToken);
    }

    public async Task RemoveFilesAsync(IEnumerable<string> files, CancellationToken cancellationToken = default)
    {
        await _minioService.DeleteItemsAsync(Bucket, files, cancellationToken);
    }
}
