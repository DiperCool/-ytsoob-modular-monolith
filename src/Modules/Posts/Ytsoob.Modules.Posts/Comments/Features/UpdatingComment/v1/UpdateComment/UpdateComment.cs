using Ardalis.GuardClauses;
using Asp.Versioning.Conventions;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Web;
using BuildingBlocks.Security.Jwt;
using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Posts.Comments.Dtos;
using Ytsoob.Modules.Posts.Comments.Exceptions;
using Ytsoob.Modules.Posts.Comments.Models;
using Ytsoob.Modules.Posts.Comments.ValueObjects;
using Ytsoob.Modules.Posts.Shared.Data;

namespace Ytsoob.Modules.Posts.Comments.Features.UpdatingComment.v1.UpdateComment;

public record UpdateComment(long CommentId, string Content) : ITxUpdateCommand;

public class UpdateCommentValidator : AbstractValidator<UpdateComment>
{
    public UpdateCommentValidator()
    {
        RuleFor(x => x.Content).NotEmpty().MaximumLength(150);
    }
}

public static class UpdateCommentEndpoint
{
    internal static IEndpointRouteBuilder MapUpdateCommentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPut($"{CommentsConfigs.PrefixUri}/{nameof(UpdateComment).Kebaberize()}", HandleAsync)
            .RequireAuthorization()
            .WithTags(CommentsConfigs.Tag)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName(nameof(UpdateComment))
            .WithDisplayName(nameof(UpdateComment).Humanize())
            .WithApiVersionSet(CommentsConfigs.VersionSet)
            .HasApiVersion(1.0);
        return endpoints;
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] UpdateComment request,
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

public class UpdateCommentHandler : ICommandHandler<UpdateComment>
{
    private PostsDbContext _postsDbContext;
    private ICurrentUserService _currentUserService;

    public UpdateCommentHandler(ICurrentUserService currentUserService, PostsDbContext postsDbContext)
    {
        _currentUserService = currentUserService;
        _postsDbContext = postsDbContext;
    }

    public async Task<Unit> Handle(UpdateComment request, CancellationToken cancellationToken)
    {
        Comment? comment = await _postsDbContext.Comments.FirstOrDefaultAsync(
            x => x.Id == request.CommentId && x.CreatedBy == _currentUserService.YtsooberId,
            cancellationToken: cancellationToken
        );
        if (comment == null)
        {
            throw new CommentNotFoundException(request.CommentId);
        }

        comment.UpdateContent(CommentContent.Of(request.Content));
        _postsDbContext.Comments.Update(comment);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
