using System.Reflection;
using BuildingBlocks.Abstractions.Messaging;
using BuildingBlocks.Core.Extensions;
using BuildingBlocks.Core.Messaging.MessagePersistence;
using Microsoft.Extensions.Configuration;
using Scrutor;

namespace BuildingBlocks.Core.Registrations;

public static class MessagingCoreRegistrationExtensions
{
    internal static void AddMessagingCore(
        this IServiceCollection services,
        IConfiguration configuration,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient,
        string? rootSectionName = null,
        params Assembly[] assembliesToScan
    )
    {
        AddPersistenceMessage(services, configuration, serviceLifetime, rootSectionName);
    }

    internal static void AddPersistenceMessage(
        IServiceCollection services,
        IConfiguration configuration,
        ServiceLifetime serviceLifetime,
        string? rootSectionName = null
    )
    {
        // services.Add<IMessagePersistenceService, NullMessagePersistenceService>(serviceLifetime);
        //  services.AddHostedService<MessagePersistenceBackgroundService>();

        var section = string.IsNullOrEmpty(rootSectionName)
            ? nameof(MessagePersistenceOptions)
            : $"{rootSectionName}:{nameof(MessagePersistenceOptions)}";

        services
            .AddOptions<MessagePersistenceOptions>()
            .Bind(configuration.GetSection(section))
            .ValidateDataAnnotations();
    }

    public static void AddMessageHandlers(
        this IServiceCollection services,
        Assembly[] assembliesToScan,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient
    )
    {
        services.Scan(scan =>
            scan.FromAssemblies(assembliesToScan.Any() ? assembliesToScan : AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(classes => classes.AssignableTo(typeof(IMessageHandler<>)))
                .UsingRegistrationStrategy(RegistrationStrategy.Append)
                .AsClosedTypeOf(typeof(IMessageHandler<>))
                .AsSelf()
                .WithLifetime(serviceLifetime)
        );
    }
}
