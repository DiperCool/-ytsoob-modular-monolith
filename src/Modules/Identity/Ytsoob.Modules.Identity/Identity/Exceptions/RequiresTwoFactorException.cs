using System.Net;
using BuildingBlocks.Core.Exception.Types;

namespace Ytsoob.Modules.Identity.Identity.Exceptions;

public class RequiresTwoFactorException : AppException
{
    public RequiresTwoFactorException(string message)
        : base(message, HttpStatusCode.BadRequest) { }
}
