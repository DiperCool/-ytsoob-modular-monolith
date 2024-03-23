using System.Net;
using BuildingBlocks.Core.Exception.Types;

namespace Ytsoob.Modules.Posts.Exceptions.Domains;

public class AlreadyVotedException : AppException
{
    public AlreadyVotedException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        : base(message, statusCode) { }

    public AlreadyVotedException(long optionId)
        : base($"Ytsoober already voted this poll with option Id = {optionId}", HttpStatusCode.BadRequest) { }
}
