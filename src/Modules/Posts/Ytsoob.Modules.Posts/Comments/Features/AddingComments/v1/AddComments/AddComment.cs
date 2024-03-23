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
using Ytsoob.Modules.Posts.Comments.Models;
using Ytsoob.Modules.Posts.Comments.ValueObjects;
using Ytsoob.Modules.Posts.Posts.Exception;
using Ytsoob.Modules.Posts.Posts.ValueObjects;
using Ytsoob.Modules.Posts.Reactions.Models;
using Ytsoob.Modules.Posts.Shared.Data;

namespace Ytsoob.Modules.Posts.Comments.Features.AddingComments.v1.AddComments;

public record AddComment(long PostId, string Content) : ITxCreateCommand<CommentDto>;

public class AddCommentValidator : AbstractValidator<AddComment>
{
    public AddCommentValidator()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(150);
    }
}

public static class AddCommentEndpoint
{
    internal static IEndpointRouteBuilder MapAddCommentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPost($"{CommentsConfigs.PrefixUri}/{nameof(AddComment).Kebaberize()}", HandleAsync)
            .RequireAuthorization()
            .WithTags(CommentsConfigs.Tag)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName(nameof(AddComment))
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

public class AddCommentHandler : ICommandHandler<AddComment, CommentDto>
{
    private PostsDbContext _postsDbContext;
    private IMapper _mapper;

    public AddCommentHandler(PostsDbContext postsDbContext, IMapper mapper)
    {
        _postsDbContext = postsDbContext;
        _mapper = mapper;
    }

    public async Task<CommentDto> Handle(AddComment request, CancellationToken cancellationToken)
    {
        if (!await _postsDbContext.Posts.AnyAsync(x => x.Id == request.PostId, cancellationToken: cancellationToken))
        {
            throw new PostNotFoundException(request.PostId);
        }

        Comment comment = Comment.Create(
            CommentId.Of(SnowFlakIdGenerator.NewId()),
            PostId.Of(request.PostId),
            CommentContent.Of(request.Content),
            ReactionStats.Create(SnowFlakIdGenerator.NewId())
        );
        await _postsDbContext.BaseComments.AddAsync(comment, cancellationToken);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<Comment, CommentDto>(comment);
    }
}
