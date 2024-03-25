using BuildingBlocks.Abstractions.Messaging;
using DotNetCore.CAP;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Cap;

public class CapBus : IBus
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<CapBus> _logger;
    private readonly ICapPublisher _capPublisher;

    public CapBus(IServiceProvider serviceProvider, ILogger<CapBus> logger, ICapPublisher capPublisher)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _capPublisher = capPublisher;
    }

    public async Task PublishAsync<TMessage>(
        TMessage message,
        IDictionary<string, string>? headers = null,
        CancellationToken cancellationToken = default
    )
        where TMessage : class, IMessage
    {
        await PublishAsync(message, headers, cancellationToken);
    }

    public async Task PublishAsync(
        object message,
        IDictionary<string, string>? headers = null,
        CancellationToken cancellationToken = default
    )
    {
        await _capPublisher.PublishAsync(
            message.GetType().Name,
            message,
            headers: headers ?? new Dictionary<string, string>(),
            cancellationToken: cancellationToken
        );
    }
}
