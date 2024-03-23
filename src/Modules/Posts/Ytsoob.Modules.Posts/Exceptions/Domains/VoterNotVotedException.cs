using System.Net;
using BuildingBlocks.Core.Exception.Types;

namespace Ytsoob.Modules.Posts.Exceptions.Domains;

public class VoterNotVotedException : AppException
{
    public VoterNotVotedException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        : base(message, statusCode) { }

    public VoterNotVotedException(long optionId)
        : base($"Ytsoober doesn't vote this option Id = {optionId}", HttpStatusCode.BadRequest) { }
}
