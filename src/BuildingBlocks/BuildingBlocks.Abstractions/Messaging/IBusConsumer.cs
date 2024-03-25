namespace BuildingBlocks.Abstractions.Messaging;

public interface IBusConsumer
{
    /// <summary>
    /// consume the message, with specified subscribeMethod.
    /// </summary>
    /// <param name="handler">handler to execute the message.</param>
    /// <param name="consumeBuilder"></param>
    /// <typeparam name="TMessage">A type that implements the <see cref="IMessage"/>.</typeparam>
    void Consume<TMessage>(
        IMessageHandler<TMessage> handler,
        Action<IConsumeConfigurationBuilder>? consumeBuilder = null
    )
        where TMessage : class, IMessage;

    /// <summary>
    /// consume the message, with specified subscribeMethod.
    /// </summary>
    /// <param name="subscribeMethod">The delegate handler to execute the message.</param>
    /// <param name="consumeBuilder"></param>
    /// <typeparam name="TMessage">A type that implements the <see cref="IMessage"/>.</typeparam>
    void Consume<TMessage>(
        MessageHandler<TMessage> subscribeMethod,
        Action<IConsumeConfigurationBuilder>? consumeBuilder = null
    )
        where TMessage : class, IMessage;

    /// <summary>
    /// Consume a message with <see cref="TMessage"/> message type and discovering a message handler that implements <see cref="IMessageHandler{TMessage}"/> interface for this type.
    /// </summary>
    /// <typeparam name="TMessage">A type that implements the <see cref="IMessage"/>.</typeparam>
    void Consume<TMessage>()
        where TMessage : class, IMessage;

    /// <summary>
    /// Consume a message with specific message type with discovering a message handler that implements <see cref="IMessageHandler{TMessage}"/> interface for this type.
    /// </summary>
    /// <param name="messageType"></param>
    void Consume(Type messageType);

    /// <summary>
    /// Consume a message with <see cref="TMessage"/> type and <see cref="THandler"/> handler.
    /// </summary>
    /// <typeparam name="THandler">A type that implements the <see cref="IMessageHandler{TMessage}"/> interface.</typeparam>
    /// <typeparam name="TMessage">A type that implements the <see cref="IMessage"/>.</typeparam>
    void Consume<THandler, TMessage>()
        where THandler : IMessageHandler<TMessage>
        where TMessage : class, IMessage;
}
