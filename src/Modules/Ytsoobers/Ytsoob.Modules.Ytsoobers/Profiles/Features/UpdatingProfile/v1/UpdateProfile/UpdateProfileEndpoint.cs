using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Humanizer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
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
