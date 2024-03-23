using Ardalis.GuardClauses;
using Asp.Versioning.Conventions;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Web;
using BuildingBlocks.Core.IdsGenerator;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Humanizer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Posts.Comments.Dtos;
using Ytsoob.Modules.Posts.Comments.Exceptions;
using Ytsoob.Modules.Posts.Comments.Features.AddingComments.v1.AddComments;
using Ytsoob.Modules.Posts.Comments.Models;
using Ytsoob.Modules.Posts.Comments.ValueObjects;
using Ytsoob.Modules.Posts.Posts.Exception;
using Ytsoob.Modules.Posts.Posts.ValueObjects;
using Ytsoob.Modules.Posts.Reactions.Models;
using Ytsoob.Modules.Posts.Shared.Data;

namespace Ytsoob.Modules.Posts.Comments.Features.ReplyComment.v1.ReplyComment;

public record ReplyComment(long PostId, long CommentId, long ReplyToCommentId, string Content)
    : ITxCreateCommand<CommentDto>;

public class ReplyCommentValidator : AbstractValidator<ReplyComment>
{
    public ReplyCommentValidator()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(150);
    }
}

public static class ReplyCommentEndpoint
{
    internal static IEndpointRouteBuilder MapReplyCommentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPost($"{CommentsConfigs.PrefixUri}/{nameof(ReplyComment).Kebaberize()}", HandleAsync)
            .RequireAuthorization()
            .WithTags(CommentsConfigs.Tag)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName(nameof(ReplyComment))
            .WithDisplayName(nameof(AddComment).Humanize())
            .WithApiVersionSet(CommentsConfigs.VersionSet)
            .HasApiVersion(1.0);
        return endpoints;
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] AddComment request,
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

public class ReplyCommentHandler : ICommandHandler<ReplyComment, CommentDto>
{
    private PostsDbContext _postsDbContext;
    private IMapper _mapper;

    public ReplyCommentHandler(PostsDbContext postsDbContext, IMapper mapper)
    {
        _postsDbContext = postsDbContext;
        _mapper = mapper;
    }

    public async Task<CommentDto> Handle(ReplyComment request, CancellationToken cancellationToken)
    {
        if (!await _postsDbContext.Posts.AnyAsync(x => x.Id == request.PostId, cancellationToken: cancellationToken))
        {
            throw new PostNotFoundException(request.PostId);
        }

        if (
            !await _postsDbContext.Comments.AnyAsync(
                x => x.Id == request.CommentId && x.PostId == request.PostId,
                cancellationToken: cancellationToken
            )
        )
        {
            throw new CommentNotFoundException(request.CommentId);
        }

        bool replyToRepliedComment = request.CommentId != request.ReplyToCommentId;
        bool repliedCommentExistInComment = !await _postsDbContext.RepliedComments.AnyAsync(
            x => x.CommentId == request.CommentId && x.Id == request.ReplyToCommentId,
            cancellationToken: cancellationToken
        );
        if (replyToRepliedComment && repliedCommentExistInComment)
        {
            throw new CommentNotFoundException(request.ReplyToCommentId);
        }

        RepliedComment comment = RepliedComment.Create(
            CommentId.Of(SnowFlakIdGenerator.NewId()),
            CommentId.Of(request.CommentId),
            PostId.Of(request.PostId),
            CommentId.Of(request.ReplyToCommentId),
            CommentContent.Of(request.Content),
            ReactionStats.Create(SnowFlakIdGenerator.NewId())
        );
        await _postsDbContext.BaseComments.AddAsync(comment, cancellationToken);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<RepliedComment, CommentDto>(comment);
    }
}
