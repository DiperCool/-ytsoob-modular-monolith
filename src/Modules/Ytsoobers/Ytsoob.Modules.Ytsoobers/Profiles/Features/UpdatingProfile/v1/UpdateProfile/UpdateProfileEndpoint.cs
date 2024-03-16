using Asp.Versioning.Conventions;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.CQRS.Query;
using BuildingBlocks.Abstractions.Web.MinimalApi;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Humanizer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Ytsoob.Modules.Ytsoobers.Profiles.Dtos.v1;
using Ytsoob.Modules.Ytsoobers.Profiles.Features.GettingProfile.v1.GetProfile;
using Ytsoob.Modules.Ytsoobers.Ytsoobers.ValueObjects;

namespace Ytsoob.Modules.Ytsoobers.Profiles.Features.UpdatingProfile.v1.UpdateProfile;

public static class GetProfileEndpoint
{
    internal static IEndpointRouteBuilder MapUpdateProfileEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPut($"{ProfilesConfig.ProfilesPrefixUri}/{nameof(UpdateProfile).Kebaberize()}", GetProfile)
            .RequireAuthorization()
            .WithTags(ProfilesConfig.Tag)
            .Produces(StatusCodes.Status201Created)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName(nameof(UpdateProfile))
            .WithDisplayName(nameof(UpdateProfile).Humanize())
            .WithApiVersionSet(ProfilesConfig.VersionSet)
            .HasApiVersion(1.0);
        return endpoints;
    }

    private static async Task<IResult> GetProfile(
        [FromBody] UpdateProfileRequest request,
        [FromServices] ICommandProcessor commandProcessor,
        [FromServices] ICurrentUserService currentUser,
        CancellationToken cancellationToken
    )
    {
        {
            var result = await commandProcessor.SendAsync(
                new UpdateProfile(
                    request.FirstName,
                    request.FirstName,
                    request.Avatar,
                    YtsooberId.Of(currentUser.YtsooberId)
                ),
                cancellationToken
            );

            return Results.NoContent();
        }
    }
}
