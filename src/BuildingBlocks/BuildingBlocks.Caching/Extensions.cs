using System.Reflection;
using Ardalis.GuardClauses;
using BuildingBlocks.Abstractions.Caching;
using BuildingBlocks.Core.Extensions;
using BuildingBlocks.Core.Utils;
using EasyCaching.Redis;
using Microsoft.Extensions.Configuration;

namespace BuildingBlocks.Caching;

public static class Extensions
{
    public static IServiceCollection AddCustomCaching(
        this IServiceCollection services,
        IConfiguration configuration,
        string moduleName,
        params Assembly[] scanAssemblies
    )
    {
        // https://www.twilio.com/blog/provide-default-configuration-to-dotnet-applications
        var cacheOptions = configuration.GetOptions<CacheOptions>($"{moduleName}:{nameof(CacheOptions)}");
        Guard.Against.Null(cacheOptions);

        AddCachingRequests(services, scanAssemblies);

        services.AddEasyCaching(option =>
        {
            if (cacheOptions.RedisCacheOptions is not null)
            {
                option.UseRedis(
                    config =>
                    {
                        config.DBConfig = new RedisDBOptions
                        {
                            Configuration = cacheOptions.RedisCacheOptions.ConnectionString
                        };
                        config.SerializerName = cacheOptions.SerializationType;
                    },
                    nameof(CacheProviderType.Redis)
                );
            }

            option.UseInMemory(
                config =>
                {
                    config.SerializerName = cacheOptions.SerializationType;
                },
                nameof(CacheProviderType.InMemory)
            );

            if (cacheOptions.SerializationType == nameof(CacheSerializationType.Json))
            {
                option.WithJson(
                    jsonSerializerSettingsConfigure: x =>
                    {
                        x.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.None;
                    },
                    nameof(CacheSerializationType.Json)
                );
            }
            else if (cacheOptions.SerializationType == nameof(CacheSerializationType.MessagePack))
            {
                option.WithMessagePack(nameof(CacheSerializationType.MessagePack));
            }
        });

        return services;
    }

    private static IServiceCollection AddCachingRequests(
        this IServiceCollection services,
        params Assembly[] scanAssemblies
    )
    {
        // Assemblies are lazy loaded so using AppDomain.GetAssemblies is not reliable (it is possible to get ReflectionTypeLoadException, because some dependent type assembly are lazy and not loaded yet), so we use `GetAllReferencedAssemblies` and it
        // load all referenced assemblies explicitly.
        var assemblies = scanAssemblies.Any()
            ? scanAssemblies
            : ReflectionUtilities.GetReferencedAssemblies(Assembly.GetCallingAssembly()).ToArray();

        // ICacheRequest discovery and registration
        services.Scan(scan =>
            scan.FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo(typeof(ICacheRequest<,>)), false)
                .AsImplementedInterfaces()
                .WithTransientLifetime()
        );

        // IInvalidateCacheRequest discovery and registration
        services.Scan(scan =>
            scan.FromAssemblies(assemblies)
                .AddClasses(classes => classes.AssignableTo(typeof(IInvalidateCacheRequest<,>)), false)
                .AsImplementedInterfaces()
                .WithTransientLifetime()
        );

        return services;
    }
}
