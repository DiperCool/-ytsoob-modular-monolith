using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Ytsoob.Modules.Posts.Polls.Feature.GettingVoters.v1.GetVoter;
using Ytsoob.Modules.Posts.Polls.Feature.Unvoting.v1.Unvote;
using Ytsoob.Modules.Posts.Polls.Feature.Voting.v1.Vote;
using Ytsoob.Modules.Posts.Posts.Features.AddingReaction.v1.AddReaction;
using Ytsoob.Modules.Posts.Posts.Features.CreatingPost.v1;
using Ytsoob.Modules.Posts.Posts.Features.DeletingPost;
using Ytsoob.Modules.Posts.Posts.Features.GettingPosts.v1.GetPosts;
using Ytsoob.Modules.Posts.Posts.Features.GettingReactions.v1.GetReactions;
using Ytsoob.Modules.Posts.Posts.Features.RemovingReaction.v1.RemoveReaction;

namespace Ytsoob.Modules.Posts.Polls;

internal static class PollConfigs
{
    public const string Tag = "Polls";
    public const string PrefixUri = $"{PostsModuleConfiguration.PostsModulePrefixUri}/polls";
    public static ApiVersionSet VersionSet { get; private set; } = default!;

    internal static IEndpointRouteBuilder MapPollEndpoints(this IEndpointRouteBuilder endpoints)
    {
        VersionSet = endpoints.NewApiVersionSet(Tag).Build();

        // create a new sub group for each version

        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0#route-groups
        // https://github.com/dotnet/aspnet-api-versioning/blob/main/examples/AspNetCore/WebApi/MinimalOpenApiExample/Program.cs

        endpoints.MapGetVotersEndpoint();
        endpoints.MapVoteEndpoint();
        endpoints.MapUnvoteEndpoint();
        return endpoints;
    }
}
