using BuildingBlocks.BlobStorage.Policies;
using BuildingBlocks.Core.Web.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Minio;

namespace BuildingBlocks.BlobStorage;

public class MinioService : IMinioService
{
    private readonly ILogger<MinioService> _logger;
    private readonly MinioClient _minioClient;
    private readonly MinioOptions _options;

    public MinioService(MinioClient minioClient, ILogger<MinioService> logger, IConfiguration configuration)
    {
        _minioClient = minioClient;
        _logger = logger;
        _options = configuration.BindOptions<MinioOptions>();
    }

    public async Task<string?> AddItemAsync(
        string bucket,
        IFormFile item,
        CancellationToken cancellationToken = default
    )
    {
        Guid id = Guid.NewGuid();
        var fileExtension = Path.GetExtension(item.FileName);
        var uniqueFileName = $"{id}{fileExtension}";

        await _minioClient.PutObjectAsync(
            new PutObjectArgs()
                .WithBucket(bucket)
                .WithObject(uniqueFileName)
                .WithObjectSize(item.Length)
                .WithStreamData(item.OpenReadStream()),
            cancellationToken
        );
        _logger.LogInformation(
            "Item added successfully File = {UniqueFileName}, Bucket = {Bucket}",
            uniqueFileName,
            bucket
        );
        var fileUrl = $"http://{_options.Uri}/{bucket}/{uniqueFileName}";
        return fileUrl;
    }

    public async Task<IEnumerable<string?>> AddItemsAsync(
        string bucket,
        IEnumerable<IFormFile> items,
        CancellationToken cancellationToken = default
    )
    {
        var tasks = items.Select(item => AddItemAsync(bucket, item, cancellationToken)).ToList();
        return await Task.WhenAll(tasks);
    }

    public async Task<string?> CreateBucketIfNotExistsAsync(
        string bucket,
        IBucketPolicy? policy,
        CancellationToken cancellationToken = default
    )
    {
        if (!await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket), cancellationToken))
        {
            _logger.LogInformation("Bucket created with name = {Bucket}", bucket);
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucket), cancellationToken);
            string policyStr = policy?.Get(bucket) ?? "";
            await _minioClient.SetPolicyAsync(
                new SetPolicyArgs().WithBucket(bucket).WithPolicy(policyStr),
                cancellationToken
            );
        }

        return bucket;
    }

    public async Task<bool> DeleteItemAsync(string bucket, string item, CancellationToken cancellationToken = default)
    {
        await _minioClient.RemoveObjectAsync(
            new RemoveObjectArgs().WithBucket(bucket).WithObject(item),
            cancellationToken
        );
        return true;
    }

    public async Task<bool> DeleteItemsAsync(
        string bucket,
        IEnumerable<string> items,
        CancellationToken cancellationToken = default
    )
    {
        var tasks = items.Select(item => DeleteItemAsync(bucket, item, cancellationToken)).ToList();
        await Task.WhenAll(tasks);
        return true;
    }
}
