using System.Reflection;
using BuildingBlocks.Caching.InMemory;
using BuildingBlocks.Core.Caching;
using BuildingBlocks.Core.IdsGenerator;
using BuildingBlocks.Core.Persistence.EfCore;
using BuildingBlocks.Core.Registrations;
using BuildingBlocks.Logging;
using BuildingBlocks.Persistence.EfCore.Postgres;
using BuildingBlocks.Security;
using BuildingBlocks.Security.Jwt;
using BuildingBlocks.Validation;
using BuildingBlocks.Web.Extensions.ServiceCollectionExtensions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ytsoob.Modules.Ytsoobers.Shared.Data;

namespace Ytsoob.Modules.Ytsoobers.Shared.Extensions.ServiceCollectionExtensions;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        SnowFlakIdGenerator.Configure(2);
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddControllersAsServices();
        services.AddCqrs(doMoreActions: s =>
        {
            s.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>))
                .AddScoped(typeof(IStreamPipelineBehavior<,>), typeof(StreamRequestValidationBehavior<,>))
                .AddScoped(typeof(IStreamPipelineBehavior<,>), typeof(StreamLoggingBehavior<,>))
                .AddScoped(typeof(IStreamPipelineBehavior<,>), typeof(StreamCachingBehavior<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(InvalidateCachingBehavior<,>))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(EfTxBehavior<,>));
        });

        services.AddCore(configuration, YtsoobersModuleConfiguration.ModuleName, Assembly.GetExecutingAssembly());

        services.AddCustomValidators(Assembly.GetExecutingAssembly());

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddJwt(configuration);

        services.AddInMemoryMessagePersistence();
        services.AddInMemoryCommandScheduler();
        services.AddInMemoryBroker(configuration);

        services.AddCustomInMemoryCache(configuration).AddCachingRequestPolicies(Assembly.GetExecutingAssembly());

        services.AddSingleton<ILoggerFactory>(new Serilog.Extensions.Logging.SerilogLoggerFactory());
        services.AddPostgresDbContext<YtsoobersDbContext>(
            configuration,
            $"{YtsoobersModuleConfiguration.ModuleName}:{nameof(PostgresOptions)}"
        );
        return services;
    }
}
