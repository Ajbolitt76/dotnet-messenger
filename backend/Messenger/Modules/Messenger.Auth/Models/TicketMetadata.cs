namespace Messenger.Auth.Models;

public record TicketMetadata(string Phone, PhoneTicketScopes Scope)
{
    public string GetRedisKey() => $"{Phone}:{Scope}";
}