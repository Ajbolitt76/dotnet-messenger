namespace Messenger.Auth.Models;

public record PersistedPhoneTicketRequest(
    string PhoneNumber,
    string Code,
    DateTime NextTry,
    PhoneTicketScopes Scope);