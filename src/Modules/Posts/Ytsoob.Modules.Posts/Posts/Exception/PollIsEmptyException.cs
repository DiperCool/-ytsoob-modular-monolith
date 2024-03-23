using System.Net;
using BuildingBlocks.Core.Domain.Exceptions;
using Ytsoob.Modules.Posts.Posts.ValueObjects;

namespace Ytsoob.Modules.Posts.Posts.Exception;

public class PollIsEmptyException : DomainException
{
    public PollIsEmptyException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        : base(message, statusCode) { }

    public PollIsEmptyException(PostId postId)
        : base($"Poll is empty in Post Id = {postId}", HttpStatusCode.BadRequest) { }
}
