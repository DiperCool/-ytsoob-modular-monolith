using Ardalis.GuardClauses;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Web;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Core.CQRS.Event.Internal;
using BuildingBlocks.Security.Jwt;
using FluentValidation;
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

namespace Ytsoob.Modules.Subscriptions.Subscriptions.Features.UpdatingSubsctiption.v1.UpdateSubsctiption;

public record SubscriptionUpdatedDomainEvent(long Id, string Title, string Description, string? Photo) : DomainEvent;

public record UpdateSubscription(long Id, string Title, string Description) : ITxUpdateCommand;

public class UpdateSubscriptionValidator : AbstractValidator<UpdateSubscription>
{
    public UpdateSubscriptionValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(300);
    }
}

public class UpdateSubscriptionEndpoint : IMinimalEndpoint
{
    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromBody] UpdateSubscription request,
        IGatewayProcessor<SubscriptionsModuleConfiguration> commandProcessor,
        CancellationToken cancellationToken
    )
    {
        Guard.Against.Null(request, nameof(request));

        using (Serilog.Context.LogContext.PushProperty("Endpoint", nameof(UpdateSubscriptionEndpoint)))
        using (Serilog.Context.LogContext.PushProperty(nameof(SubscriptionId), request.Id))
        {
            await commandProcessor.ExecuteCommand<UpdateSubscription, Unit>(request, cancellationToken);

            return Results.NoContent();
        }
    }

    public string GroupName => SubscriptionsConfigs.Tag;
    public string PrefixRoute => SubscriptionsConfigs.PrefixUri;
    public double Version => 1.0;

    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapPut(nameof(UpdateSubscription).Kebaberize(), HandleAsync)
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName(nameof(UpdateSubscription))
            .WithDisplayName(nameof(UpdateSubscription).Pluralize());
    }
}

public class UpdateSubscriptionHandler : ICommandHandler<UpdateSubscription>
{
    private ICurrentUserService _currentUserService;
    private ISubscriptionsDbContext _subscriptionsDbContext;

    public UpdateSubscriptionHandler(
        ISubscriptionsDbContext subscriptionsDbContext,
        ICurrentUserService currentUserService
    )
    {
        _subscriptionsDbContext = subscriptionsDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(UpdateSubscription request, CancellationToken cancellationToken)
    {
        Subscription? subscription = await _subscriptionsDbContext.Subscriptions.FirstOrDefaultAsync(
            x => x.Id == request.Id && x.CreatedBy == _currentUserService.YtsooberId,
            cancellationToken: cancellationToken
        );
        if (subscription == null)
            throw new SubscriptionNotFoundException(request.Id);
        subscription.Update(Title.Of(request.Title), Description.Of(request.Description), subscription.Photo);
        _subscriptionsDbContext.Subscriptions.Update(subscription);
        await _subscriptionsDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
