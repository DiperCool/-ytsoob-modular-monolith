namespace BuildingBlocks.Abstractions.Messaging;

// Ref: https://www.enterpriseintegrationpatterns.com/patterns/messaging/EnvelopeWrapper.html
public class MessageEnvelope
{
    public MessageEnvelope(object message, IDictionary<string, string>? headers = null)
    {
        Message = message;
        Headers = headers ?? new Dictionary<string, string>();
    }

    public object Message { get; init; }
    public IDictionary<string, string> Headers { get; init; }
}

public class MessageEnvelope<TMessage> : MessageEnvelope
    where TMessage : class, IMessage
{
    public MessageEnvelope(TMessage message, IDictionary<string, string> header)
        : base(message, header)
    {
        Message = message;
    }

    public new TMessage Message { get; }
}
