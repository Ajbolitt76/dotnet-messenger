namespace Messenger.Auth.Features.PhoneTicket.RequestPhoneTicket;

public record RequestPhoneTicketResponse(DateTime NextTry, bool CodeSent);
