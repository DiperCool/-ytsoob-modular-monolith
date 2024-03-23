using System.Net;
using BuildingBlocks.Core.Exception.Types;

namespace Ytsoob.Modules.Posts.Posts.Exception;

public class PostNotFoundException : AppException
{
    public PostNotFoundException(long id)
        : base($"Product with id '{id}' not found", HttpStatusCode.NotFound) { }

    public PostNotFoundException(string message)
        : base(message, HttpStatusCode.NotFound) { }
}
