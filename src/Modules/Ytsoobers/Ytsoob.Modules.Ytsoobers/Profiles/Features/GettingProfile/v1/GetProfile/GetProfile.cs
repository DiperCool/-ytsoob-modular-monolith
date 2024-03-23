using Ardalis.GuardClauses;
using Asp.Versioning.Conventions;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Query;
using BuildingBlocks.Abstractions.Web;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Humanizer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Ytsoobers.Profiles.Dtos.v1;
using Ytsoob.Modules.Ytsoobers.Shared.Data;
using Ytsoob.Modules.Ytsoobers.Ytsoobers.Models;
using Ytsoob.Modules.Ytsoobers.Ytsoobers.ValueObjects;

namespace Ytsoob.Modules.Ytsoobers.Profiles.Features.GettingProfile.v1.GetProfile;

public record GetProfile(YtsooberId YtsooberId) : IQuery<ProfileDto>;

public static class GetProfileEndpoint
{
    internal static IEndpointRouteBuilder MapGetProfileEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGet($"{ProfilesConfig.ProfilesPrefixUri}/{nameof(v1.GetProfile.GetProfile).Kebaberize()}", GetProfile)
            .WithTags(ProfilesConfig.Tag)
            .Produces<ProfileDto>()
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName("GetProfile")
            .WithDisplayName("Get profile.")
            .WithApiVersionSet(ProfilesConfig.VersionSet)
            .HasApiVersion(1.0);
        return endpoints;
    }

    private static async Task<IResult> GetProfile(
        [FromServices] IGatewayProcessor<YtsoobersModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken
    )
    {
        return await gatewayProcessor.ExecuteScopeAsync(async sp =>
        {
            var currentUser = sp.GetRequiredService<ICurrentUserService>();
            var result = await sp.GetRequiredService<IQueryProcessor>()
                .SendAsync(new GetProfile(YtsooberId.Of(currentUser.YtsooberId)), cancellationToken);

            return Results.Ok(result);
        });
    }
}

public class GetProfileHandler : IQueryHandler<GetProfile, ProfileDto>
{
    private YtsoobersDbContext _ytsoobersDbContext;
    private IMapper _mapper;

    public GetProfileHandler(YtsoobersDbContext ytsoobersDbContext, IMapper mapper)
    {
        _ytsoobersDbContext = ytsoobersDbContext;
        _mapper = mapper;
    }

    public async Task<ProfileDto> Handle(GetProfile request, CancellationToken cancellationToken)
    {
        Ytsoober? ytsoober = await _ytsoobersDbContext
            .Ytsoobers.Include(x => x.Profile)
            .FirstOrDefaultAsync(x => x.Id == request.YtsooberId, cancellationToken: cancellationToken);
        Guard.Against.Null(ytsoober);
        return _mapper.Map<ProfileDto>(ytsoober.Profile);
    }
}
