using Asp.Versioning.Conventions;
using BuildingBlocks.Abstractions.Web;
using Hellang.Middleware.ProblemDetails;
using Humanizer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Ytsoob.Modules.Posts.Posts.Features.DeletingPost;

public static class DeletePostEndpoint
{
    internal static IEndpointRouteBuilder MapDeletePostEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapDelete($"{PostsConfigs.PostsPrefixUri}/{nameof(DeletePost).Kebaberize()}", HandleAsync)
            .RequireAuthorization()
            .WithTags(PostsConfigs.Tag)
            .Produces(StatusCodes.Status201Created)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName(nameof(DeletePost))
            .WithDisplayName(nameof(DeletePost).Humanize())
            .WithApiVersionSet(PostsConfigs.VersionSet)
            .HasApiVersion(1.0);
        return endpoints;
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] DeletePost request,
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
