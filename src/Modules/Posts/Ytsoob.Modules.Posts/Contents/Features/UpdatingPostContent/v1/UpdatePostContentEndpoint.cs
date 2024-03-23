using Ardalis.GuardClauses;
using Asp.Versioning.Conventions;
using AutoMapper;
using BuildingBlocks.Abstractions.Web;
using Hellang.Middleware.ProblemDetails;
using Humanizer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Ytsoob.Modules.Posts.Contents.Features.UpdatingPostContent.v1;

public static class UpdatePostContentEndpoint
{
    internal static IEndpointRouteBuilder MapUpdatePostContentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPut($"{ContentsConfig.PostsPrefixUri}/{nameof(UpdatePostContent).Kebaberize()}", HandleAsync)
            .RequireAuthorization()
            .WithTags(ContentsConfig.Tag)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName(nameof(UpdatePostContent))
            .WithDisplayName(nameof(UpdatePostContent).Humanize())
            .WithApiVersionSet(ContentsConfig.VersionSet)
            .HasApiVersion(1.0);
        return endpoints;
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] UpdatePostContent request,
        IGatewayProcessor<PostsModuleConfiguration> gatewayProcessor,
        CancellationToken cancellationToken
    )
    {
        return await gatewayProcessor.ExecuteCommand(async commandProcessor =>
        {
            await commandProcessor.SendAsync(request, cancellationToken);

            return Results.NoContent();
        });
    }
}
