using Ardalis.GuardClauses;
using Asp.Versioning.Conventions;
using AutoMapper;
using BuildingBlocks.Abstractions.CQRS.Command;
using BuildingBlocks.Abstractions.Web;
using BuildingBlocks.Security.Jwt;
using Hellang.Middleware.ProblemDetails;
using Humanizer;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Posts.Exceptions;
using Ytsoob.Modules.Posts.Polls.Models;
using Ytsoob.Modules.Posts.Posts.Exception;
using Ytsoob.Modules.Posts.Posts.Models;
using Ytsoob.Modules.Posts.Shared.Data;
using Ytsoob.Modules.Posts.Ytsoobers.Models;

namespace Ytsoob.Modules.Posts.Polls.Feature.Voting.v1.Vote;

public record Vote(long PostId, long OptionId) : ITxCreateCommand;

public static class VoteEndpoint
{
    internal static IEndpointRouteBuilder MapVoteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPost($"{PollConfigs.PrefixUri}/{nameof(Vote).Kebaberize()}", HandleAsync)
            .AllowAnonymous()
            .WithTags(PollConfigs.Tag)
            .Produces<StatusCodeProblemDetails>(StatusCodes.Status400BadRequest)
            .WithName(nameof(Vote))
            .WithDisplayName(nameof(Vote).Humanize())
            .WithApiVersionSet(PollConfigs.VersionSet)
            .HasApiVersion(1.0);
        return endpoints;
    }

    private static async Task<IResult> HandleAsync(
        [FromBody] Vote request,
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

public class VoteHandler : ICommandHandler<Vote>
{
    private PostsDbContext _postsDbContext;
    private ICurrentUserService _currentUserService;

    public VoteHandler(PostsDbContext postsDbContext, ICurrentUserService currentUserService)
    {
        _postsDbContext = postsDbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(Vote request, CancellationToken cancellationToken)
    {
        Post? post = await _postsDbContext
            .Posts.Include(x => x.Poll)
            .ThenInclude(x => x.Options)
            .FirstOrDefaultAsync(x => x.Id == request.PostId, cancellationToken: cancellationToken);
        if (post == null)
            throw new PostNotFoundException(request.PostId);

        Option? option = await _postsDbContext.Options.FirstOrDefaultAsync(
            x => x.Id == request.OptionId,
            cancellationToken: cancellationToken
        );
        if (option == null)
            throw new OptionNotFoundException(request.OptionId);
        Ytsoober? ytsoober = await _postsDbContext.Ytsoobers.FirstOrDefaultAsync(
            x => x.Id == _currentUserService.YtsooberId,
            cancellationToken: cancellationToken
        );
        if (ytsoober == null)
            throw new YtsooberNotFoundException(_currentUserService.YtsooberId);
        post.VotePoll(ytsoober, option);
        _postsDbContext.Posts.Update(post);
        await _postsDbContext.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
