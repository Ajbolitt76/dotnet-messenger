using System.Net;

namespace Messenger.Core.Exceptions;

public class ForbiddenException : DomainException
{
    public ForbiddenException() : base(ErrorCodes.ForbiddenError, (int)HttpStatusCode.Forbidden) { }
    
    public ForbiddenException(string message) : base(message, (int)HttpStatusCode.Forbidden) {}
}
