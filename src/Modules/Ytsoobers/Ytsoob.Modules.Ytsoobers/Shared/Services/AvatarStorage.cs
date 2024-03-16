using BuildingBlocks.BlobStorage;
using BuildingBlocks.BlobStorage.Policies;
using Microsoft.AspNetCore.Http;
using Ytsoob.Modules.Ytsoobers.Shared.Contracts;

namespace Ytsoob.Modules.Ytsoobers.Shared.Services;

public class AvatarStorage : IAvatarStorage
{
    private IMinioService _minioService;
    private const string BucketName = "avatars";

    public AvatarStorage(IMinioService minioService)
    {
        _minioService = minioService;
    }

    public async Task<string?> UploadAvatarAsync(IFormFile file, CancellationToken cancellationToken)
    {
        await _minioService.CreateBucketIfNotExistsAsync(BucketName, new ReadonlyBucketPolicy(), cancellationToken);
        return await _minioService.AddItemAsync(BucketName, file, cancellationToken);
    }
}
