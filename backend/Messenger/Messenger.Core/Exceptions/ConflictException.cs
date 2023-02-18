using System.Net;

namespace Messenger.Core.Exceptions;

public class ConflictException : DomainException
{
    public ConflictException(string message) : base(message, (int)HttpStatusCode.Conflict)
    {
    }
}