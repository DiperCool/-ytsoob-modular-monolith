namespace BuildingBlocks.Abstractions.Messaging;

public interface IBusProducer
{
    public Task PublishAsync<TMessage>(
        TMessage message,
        IDictionary<string, string>? headers = null,
        CancellationToken cancellationToken = default
    )
        where TMessage : class, IMessage;

    public Task PublishAsync(
        object message,
        IDictionary<string, string>? headers = null,
        CancellationToken cancellationToken = default
    );
}
