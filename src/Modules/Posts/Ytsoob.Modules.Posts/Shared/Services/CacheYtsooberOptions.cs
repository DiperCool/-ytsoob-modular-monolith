using System.Globalization;
using EasyCaching.Core;
using Microsoft.EntityFrameworkCore;
using Ytsoob.Modules.Posts.Polls;
using Ytsoob.Modules.Posts.Shared.Contracts;
using Ytsoob.Modules.Posts.Shared.Data;

namespace Ytsoob.Modules.Posts.Shared.Services;

public class CacheYtsooberOptions : ICacheYtsooberOptions
{
    private IEasyCachingProvider _cache;
    private PostsDbContext _postsDbContext;

    public CacheYtsooberOptions(IEasyCachingProvider cache, PostsDbContext postsDbContext)
    {
        _cache = cache;
        _postsDbContext = postsDbContext;
    }

    public async Task<IEnumerable<long>> GetUsersOptionsInPollAsync(long pollId, long ytsooberId)
    {
        string key = string.Format(
            CultureInfo.InvariantCulture,
            PollCacheKeys.UserPollVotedCacheKey,
            ytsooberId,
            pollId
        );

        if (await _cache.ExistsAsync(key))
        {
            var cache = await _cache.GetAsync<IEnumerable<long>>(key);
            return cache.Value;
        }

        var optionIds = await _postsDbContext
            .Voters.Where(x => x.Option.PollId == pollId)
            .Select(x => x.OptionId)
            .Select(x => x.Value)
            .ToListAsync();
        await _cache.SetAsync(key, optionIds, TimeSpan.FromMinutes(60));
        return optionIds;
    }
}
