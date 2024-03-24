using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Ytsoob.Modules.Posts.Comments.Features.AddingCommentFiles.v1.AddCommentFiles;
using Ytsoob.Modules.Posts.Comments.Features.AddingComments.v1.AddComments;
using Ytsoob.Modules.Posts.Comments.Features.AddingReaction.v1.AddReaction;
using Ytsoob.Modules.Posts.Comments.Features.GettingComments.v1.GetComments;
using Ytsoob.Modules.Posts.Comments.Features.GettingRepliedComments.v1.GetRepliedComments;
using Ytsoob.Modules.Posts.Comments.Features.RemovingComment.v1.RemoveComment;
using Ytsoob.Modules.Posts.Comments.Features.RemovingCommentFiles.v1.RemoveComemntFiles;
using Ytsoob.Modules.Posts.Comments.Features.RemovingReaction.v1.RemoveReaction;
using Ytsoob.Modules.Posts.Comments.Features.ReplyComment.v1.ReplyComment;
using Ytsoob.Modules.Posts.Comments.Features.UpdatingComment.v1.UpdateComment;

namespace Ytsoob.Modules.Posts.Comments;

internal static class CommentsConfigs
{
    public const string Tag = "Comments";
    public const string PrefixUri = $"{PostsModuleConfiguration.PostsModulePrefixUri}/comments";
    public static ApiVersionSet VersionSet { get; private set; } = default!;

    internal static IEndpointRouteBuilder MapCommentsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        VersionSet = endpoints.NewApiVersionSet(Tag).Build();
        endpoints.MapAddCommentEndpoint();
        endpoints.MapAddCommentFiles();
        endpoints.MapAddReactionCommentEndpoint();
        endpoints.MapGetRepliedComments();
        endpoints.MapGetCommentsEndpoint();
        endpoints.MapRemoveCommentFilesEndpoint();
        endpoints.MapRemoveCommentEndpoint();
        endpoints.MapRemoveReactionCommentEndpoint();
        endpoints.MapReplyCommentEndpoint();
        endpoints.MapUpdateCommentEndpoint();

        return endpoints;
    }
}
