using System.Net;
using BuildingBlocks.Core.Exception.Types;

namespace Ytsoob.Modules.Identity.Identity.Exceptions;

public class PhoneNumberNotConfirmedException : AppException
{
    public PhoneNumberNotConfirmedException(string phone)
        : base($"The phone number '{phone}' is not confirmed yet.", HttpStatusCode.UnprocessableEntity) { }
}
