namespace Ytsoob.Modules.Posts.Shared.Contracts;

public interface ICacheYtsooberOptions
{
    public Task<IEnumerable<long>> GetUsersOptionsInPollAsync(long pollId, long ytsooberId);
}
