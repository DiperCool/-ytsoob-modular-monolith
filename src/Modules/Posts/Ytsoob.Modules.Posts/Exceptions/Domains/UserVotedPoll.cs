using System.Net;
using BuildingBlocks.Core.Exception.Types;
using Ytsoob.Modules.Posts.Polls.ValueObjects;

namespace Ytsoob.Modules.Posts.Exceptions.Domains;

public class UserVotedPoll : AppException
{
    public UserVotedPoll(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        : base(message, statusCode) { }

    public UserVotedPoll(PollId pollId)
        : base($"User voted in poll Id = {pollId}", HttpStatusCode.BadRequest) { }
}
