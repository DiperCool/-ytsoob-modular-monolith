using BuildingBlocks.Core.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.AspNetCore.HealthChecks;
using Minio.Exceptions;
using Polly;

namespace BuildingBlocks.BlobStorage;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddBlobStorage(this WebApplicationBuilder builder, string moduleName)
    {
        builder.Services.AddBlobStorage(builder.Configuration, moduleName);
        return builder;
    }

    public static IServiceCollection AddBlobStorage(
        this IServiceCollection services,
        IConfiguration configuration,
        string moduleName
    )
    {
        services.AddSingleton(provider =>
        {
            var blobStorateConfiguration = configuration.GetOptions<MinioOptions>(
                $"{moduleName}:{nameof(MinioOptions)}"
            );
            var minioClient = new MinioClient()
                .WithEndpoint(blobStorateConfiguration.Uri ?? "")
                .WithCredentials(blobStorateConfiguration.Username, blobStorateConfiguration.Password)
                .Build();
            minioClient.WithTimeout(5000);
            minioClient.WithRetryPolicy(async callback =>
                await Policy
                    .Handle<ConnectionException>()
                    .WaitAndRetryAsync(3, retryCount => TimeSpan.FromSeconds(retryCount * 2))
                    .ExecuteAsync(async () => await callback())
            );
            return minioClient;
        });
        services.AddSingleton<IMinioService, MinioService>();
        return services;
    }

    public static IServiceCollection AddBlobStorageHealthCheck(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddMinio(serviceProvider => serviceProvider.GetRequiredService<MinioClient>(), tags: new[] { "live" });
        return services;
    }
}
