using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Ytsoob.Modules.Posts.Comments.Features.GettingRepliedComments.v1.GetRepliedComments;
using Ytsoob.Modules.Posts.Contents.Features.AddingFiles.v1.AddFiles;
using Ytsoob.Modules.Posts.Contents.Features.RemovingFiles.v1.RemoveFiles;
using Ytsoob.Modules.Posts.Contents.Features.UpdatingPostContent.v1;

namespace Ytsoob.Modules.Posts.Contents;

internal static class ContentsConfig
{
    public const string Tag = "Contents";
    public const string PostsPrefixUri = $"{PostsModuleConfiguration.PostsModulePrefixUri}/contents";
    public static ApiVersionSet VersionSet { get; private set; } = default!;

    internal static IEndpointRouteBuilder MapContentsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        VersionSet = endpoints.NewApiVersionSet(Tag).Build();

        // create a new sub group for each version

        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0#route-groups
        // https://github.com/dotnet/aspnet-api-versioning/blob/main/examples/AspNetCore/WebApi/MinimalOpenApiExample/Program.cs
        endpoints.MapAddFilesEndpoint();
        endpoints.MapRemoveFilesEndpoint();
        endpoints.MapUpdatePostContentEndpoint();
        return endpoints;
    }
}
