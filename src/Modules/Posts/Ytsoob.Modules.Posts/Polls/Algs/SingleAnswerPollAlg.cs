using BuildingBlocks.Core.IdsGenerator;
using EasyCaching.Core;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Posts.Exceptions.Domains;
using Ytsoob.Modules.Posts.Polls.Models;
using Ytsoob.Modules.Posts.Polls.ValueObjects;
using Ytsoob.Modules.Posts.Shared.Contracts;
using Ytsoob.Modules.Posts.Shared.Data;

namespace Ytsoob.Modules.Posts.Polls.Algs;

public class SingleAnswerPollAlg : IPollStrategy
{
    private PostsDbContext _postsDbContext;
    private IEasyCachingProvider _cache;

    public SingleAnswerPollAlg(PostsDbContext postsDbContext, IEasyCachingProvider cache)
    {
        _postsDbContext = postsDbContext;
        _cache = cache;
    }

    public string PollType => "singlePollAnswerType";

    public async Task Vote(Poll poll, OptionId optionId, long voterId)
    {
        Voter? voter = await _postsDbContext.Voters.FirstOrDefaultAsync(x =>
            x.YtsooberId == voterId && x.Option.PollId == poll.Id
        );
        if (voter != null)
        {
            throw new AlreadyVotedException(optionId);
        }

        Voter voterCreated = new Voter(SnowFlakIdGenerator.NewId(), voterId, optionId);
        await _postsDbContext.Voters.AddAsync(voterCreated);
    }

    public async Task Unvote(Poll poll, OptionId optionId, long voterId)
    {
        Voter? voter = await _postsDbContext.Voters.FirstOrDefaultAsync(x =>
            x.YtsooberId == voterId && x.Option.Id == optionId
        );
        if (voter == null)
            throw new VoterNotVotedException(optionId);
        _postsDbContext.Voters.Remove(voter);
        await _postsDbContext.SaveChangesAsync();
    }
}
