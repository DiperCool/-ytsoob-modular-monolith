using System.Net;
using BuildingBlocks.Core.Exception.Types;

namespace Ytsoob.Modules.Identity.Identity.Exceptions;

public class PasswordIsInvalidException : AppException
{
    public PasswordIsInvalidException(string message)
        : base(message, HttpStatusCode.Forbidden) { }
}
