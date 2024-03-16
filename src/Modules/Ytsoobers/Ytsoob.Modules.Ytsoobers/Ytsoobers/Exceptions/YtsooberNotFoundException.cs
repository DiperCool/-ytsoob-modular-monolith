using System.Net;
using BuildingBlocks.Core.Exception.Types;

namespace Ytsoob.Modules.Ytsoobers.Ytsoobers.Exceptions;

public class YtsooberNotFoundException : AppException
{
    public YtsooberNotFoundException(string message, HttpStatusCode statusCode = HttpStatusCode.NotFound) : base(message, statusCode)
    {
    }

    public YtsooberNotFoundException(long id) : base($"Ytsoober with {id} not found", HttpStatusCode.NotFound)
    {
    }
}
