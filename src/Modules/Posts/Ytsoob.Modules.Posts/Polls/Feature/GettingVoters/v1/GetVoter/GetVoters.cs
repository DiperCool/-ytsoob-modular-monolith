using Asp.Versioning.Conventions;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Query;
using BuildingBlocks.Abstractions.Web;
using BuildingBlocks.Core.CQRS.Query;
using BuildingBlocks.Core.Persistence.EfCore;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Humanizer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Posts.Polls.Dtos;
using Ytsoob.Modules.Posts.Shared.Data;
using Ytsoob.Modules.Posts.Ytsoobers.Models;

namespace Ytsoob.Modules.Posts.Polls.Feature.GettingVoters.v1.GetVoter;

public record GetVotersResponse(ListResultModel<VoterDto> Voters);

public record GetVoters(long OptionId) : ListQuery<GetVotersResponse>;

public class GetPostsValidator : AbstractValidator<GetVoters>
{
    public GetPostsValidator()
    {
        CascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Page).GreaterThanOrEqualTo(1).WithMessage("Page should at least greater than or equal to 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("PageSize should at least greater than or equal to 1.");
    }
}

public static class GetVotersEndpoint
{
    internal static IEndpointRouteBuilder MapGetVotersEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGet($"{PollConfigs.PrefixUri}/{nameof(GetVoters).Kebaberize()}", HandleAsync)
            .AllowAnonymous()
            .WithTags(PollConfigs.Tag)
            .Produces<GetVotersResponse>()
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName(nameof(GetVoters))
            .WithDisplayName(nameof(GetVoters).Humanize())
            .WithApiVersionSet(PollConfigs.VersionSet)
            .HasApiVersion(1.0);
        return endpoints;
    }

    private static async Task<IResult> HandleAsync(
        long optionId,
        IGatewayProcessor<PostsModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken,
        int page = 1,
        int pageSize = 15
    )
    {
        return await gatewayProcessor.ExecuteQuery(async commandProcessor =>
        {
            var res = await commandProcessor.SendAsync(
                new GetVoters(optionId) { PageSize = pageSize, Page = page },
                cancellationToken
            );

            return Results.Ok(res);
        });
    }
}

public class GetVotersHandler : IQueryHandler<GetVoters, GetVotersResponse>
{
    private PostsDbContext _postsDbContext;
    private IMapper _mapper;

    public GetVotersHandler(PostsDbContext postsDbContext, IMapper mapper)
    {
        _postsDbContext = postsDbContext;
        _mapper = mapper;
    }

    public async Task<GetVotersResponse> Handle(GetVoters request, CancellationToken cancellationToken)
    {
        var voters = await _postsDbContext
            .Voters.AsNoTracking()
            .Where(x => x.OptionId == request.OptionId)
            .OrderByDescending(x => x.Created)
            .Select(x => x.Ytsoober)
            .ApplyPagingAsync<Ytsoober, VoterDto>(
                _mapper.ConfigurationProvider,
                request.Page,
                request.PageSize,
                cancellationToken: cancellationToken
            );

        return new GetVotersResponse(voters);
    }
}
