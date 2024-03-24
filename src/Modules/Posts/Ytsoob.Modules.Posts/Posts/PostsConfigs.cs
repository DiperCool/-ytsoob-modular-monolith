using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Ytsoob.Modules.Posts.Posts.Features.AddingReaction.v1.AddReaction;
using Ytsoob.Modules.Posts.Posts.Features.CreatingPost.v1;
using Ytsoob.Modules.Posts.Posts.Features.DeletingPost;
using Ytsoob.Modules.Posts.Posts.Features.GettingPosts.v1.GetPosts;
using Ytsoob.Modules.Posts.Posts.Features.GettingReactions.v1.GetReactions;
using Ytsoob.Modules.Posts.Posts.Features.RemovingReaction.v1.RemoveReaction;

namespace Ytsoob.Modules.Posts.Posts;

internal static class PostsConfigs
{
    public const string Tag = "Posts";
    public const string PostsPrefixUri = $"{PostsModuleConfiguration.PostsModulePrefixUri}";
    public static ApiVersionSet VersionSet { get; private set; } = default!;

    internal static IEndpointRouteBuilder MapPostsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        VersionSet = endpoints.NewApiVersionSet(Tag).Build();

        // create a new sub group for each version

        endpoints.MapAddReactEndpoint();
        endpoints.MapCreatePostEndpoint();
        endpoints.MapDeletePostEndpoint();
        endpoints.MapGetReactionsEndpoint();
        endpoints.MapRemoveReactionEndpoint();
        endpoints.MapGetPostsEndpoint();
        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0#route-groups
        // https://github.com/dotnet/aspnet-api-versioning/blob/main/examples/AspNetCore/WebApi/MinimalOpenApiExample/Program.cs


        return endpoints;
    }
}
