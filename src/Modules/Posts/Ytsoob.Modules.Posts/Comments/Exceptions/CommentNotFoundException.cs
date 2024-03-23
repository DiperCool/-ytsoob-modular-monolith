using System.Net;
using BuildingBlocks.Core.Exception.Types;

namespace Ytsoob.Modules.Posts.Comments.Exceptions;

public class CommentNotFoundException : AppException
{
    public CommentNotFoundException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        : base(message, statusCode) { }

    public CommentNotFoundException(long commentId)
        : base($"Comment not found with Id = {commentId}", HttpStatusCode.BadRequest) { }
}
