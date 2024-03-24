using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Web;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Core.CQRS.Event.Internal;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Subscriptions.Shared.Contracts;
using Ytsoob.Modules.Subscriptions.Subscriptions.Exceptions;
using Ytsoob.Modules.Subscriptions.Subscriptions.Models;
using Ytsoob.Modules.Subscriptions.Subscriptions.ValueObjects;

namespace Ytsoob.Modules.Subscriptions.Subscriptions.Features.RemovingSubscription.v1.RemoveSubscription;

public record SubscriptionRemovedDomainEvent(long Id) : DomainEvent;

public record RemoveSubscription(long Id) : ITxCommand;

public class RemoveSubscriptionEndpoint : IMinimalEndpoint
{
    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromBody] RemoveSubscription request,
        IGatewayProcessor<SubscriptionsModuleConfiguration> commandProcessor,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(request, nameof(request));

        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(RemoveSubscriptionEndpoint)))
        using (Serilog.Context.LogContext.PushProperty(nameof(SubscriptionId), request.Id))
        {
            await commandProcessor.ExecuteCommand<RemoveSubscription, Unit>(request, cancellationToken);

            return Results.NoContent();
        }
    }

    public string GroupName => SubscriptionsConfigs.Tag;
    public string PrefixRoute => SubscriptionsConfigs.PrefixUri;
    public double Version => 1.0;

    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapDelete(nameof(RemoveSubscription).Kebaberize(), HandleAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName(nameof(RemoveSubscription))
            .WithDisplayName(nameof(RemoveSubscription).Pluralize());
    }
}

public class RemoveSubscriptionHandler : ICommandHandler<RemoveSubscription>
{
    private ISubscriptionsDbContext _subscriptionsDbContext;
    private ICurrentUserService _currentUserService;

    public RemoveSubscriptionHandler(
        ICurrentUserService currentUserService,
        ISubscriptionsDbContext subscriptionsDbContext
    )
    {
        _currentUserService = currentUserService;
        _subscriptionsDbContext = subscriptionsDbContext;
    }

    public async Task<Unit> Handle(RemoveSubscription request, CancellationToken cancellationToken)
    {
        Subscription? subscription = await _subscriptionsDbContext.Subscriptions.FirstOrDefaultAsync(
            x => x.Id == request.Id && x.CreatedBy == _currentUserService.YtsooberId,
            cancellationToken: cancellationToken
        );
        if (subscription == null)
            throw new SubscriptionNotFoundException(request.Id);
        subscription.Remove();
        _subscriptionsDbContext.Subscriptions.Remove(subscription);
        await _subscriptionsDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
