using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Query;
using BuildingBlocks.Abstractions.Web;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Core.CQRS.Query;
using BuildingBlocks.Core.Persistence.EfCore;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Subscriptions.Shared.Contracts;
using Ytsoob.Modules.Subscriptions.Shared.Data;
using Ytsoob.Modules.Subscriptions.Subscriptions.Dtos;
using Ytsoob.Modules.Subscriptions.Subscriptions.Models;

namespace Ytsoob.Modules.Subscriptions.Subscriptions.Features.GettingSubscriptions.v1.GetSubscriptions;

public record GetSubscriptionsResult(ListResultModel<SubscriptionDto> Subscriptions);

public record GetSubscriptions(long YtsooberId) : ListQuery<GetSubscriptionsResult>;

public class GetPostsEndpoint : IMinimalEndpoint
{
    public string GroupName => SubscriptionsConfigs.Tag;
    public string PrefixRoute => SubscriptionsConfigs.PrefixUri;
    public double Version => 1.0;

    public RouteHandlerBuilder MapEndpoint(IEndpointRouteBuilder builder)
    {
        return builder
            .MapGet(nameof(GetSubscriptions).Kebaberize(), HandleAsync)
            .Produces<GetSubscriptionsResult>(StatusCodes.Status200OK)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status401Unauthorized)
            .WithName(nameof(GetSubscriptionsResult))
            .WithDisplayName(nameof(GetSubscriptionsResult).Humanize());
    }

    public async Task<IResult> HandleAsync(
        HttpContext context,
        [FromQuery] long ytsooberId,
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromServices] ISubscriptionsDbContext service,
        IGatewayProcessor<SubscriptionsModuleConfiguration> queryProcessor,
        CancellationToken cancellationToken
    )
    {
        GetSubscriptionsResult result = await queryProcessor.ExecuteQuery<GetSubscriptions, GetSubscriptionsResult>(
            new GetSubscriptions(ytsooberId) { Page = page, PageSize = pageSize },
            cancellationToken
        );

        return Results.Ok(result);
    }
}

public class GetSubscriptionsValidator : AbstractValidator<GetSubscriptions>
{
    public GetSubscriptionsValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Page).GreaterThanOrEqualTo(1).WithMessage("Page should at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageSize should at least greater than or equal to 1.");
    }
}

public class GetSubscriptionsHandler : IRequestHandler<GetSubscriptions, GetSubscriptionsResult>
{
    private SubscriptionsDbContext _postsDbContext;
    private IMapper _mapper;

    public GetSubscriptionsHandler(SubscriptionsDbContext postsDbContext, IMapper mapper)
    {
        _postsDbContext = postsDbContext;
        _mapper = mapper;
    }

    public async Task<GetSubscriptionsResult> Handle(GetSubscriptions request, CancellationToken cancellationToken)
    {
        var posts = await _postsDbContext
            .Subscriptions.Where(x => x.CreatedBy == request.YtsooberId)
            .OrderBy(x => x.Price)
            .AsNoTracking()
            .ApplyPagingAsync<Subscription, SubscriptionDto>(
                _mapper.ConfigurationProvider,
                request.Page,
                request.PageSize,
                cancellationToken: cancellationToken
            );

        return new GetSubscriptionsResult(posts);
    }
}
