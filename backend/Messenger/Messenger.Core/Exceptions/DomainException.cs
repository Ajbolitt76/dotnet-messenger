namespace Messenger.Core.Exceptions;

public abstract class DomainException : Exception
{
    public int StatusCode { get; set; } = 500;

    /// <summary>
    /// Данные для локализации ошибок на клиенте
    /// </summary>
    public Dictionary<string, string> PlaceholderData { get; } = new();

    public DomainException(string message) : base(message)
    {
        
    }

    public DomainException(string message, int statusCode) : this(message)
    {
        StatusCode = statusCode;
    }
}