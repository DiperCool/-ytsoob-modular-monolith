using System.Net;
using BuildingBlocks.Core.Exception.Types;

namespace Ytsoob.Modules.Subscriptions.Subscriptions.Exceptions;

public class SubscriptionNotFoundException : AppException
{
    public SubscriptionNotFoundException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        : base(message, statusCode) { }

    public SubscriptionNotFoundException(long subId)
        : base($"Subscription with this Id = {subId} not found", HttpStatusCode.BadRequest) { }
}
