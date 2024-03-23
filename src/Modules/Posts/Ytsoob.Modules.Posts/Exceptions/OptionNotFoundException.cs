using System.Net;
using BuildingBlocks.Core.Exception.Types;

namespace Ytsoob.Modules.Posts.Exceptions;

public class OptionNotFoundException : AppException
{
    public OptionNotFoundException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        : base(message, statusCode) { }

    public OptionNotFoundException(long optionId)
        : base($"Option with Id = {optionId} not found", HttpStatusCode.BadRequest) { }
}
