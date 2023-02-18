using Messenger.Auth.Features.PhoneTicket.RequestPhoneTicket;

namespace Messenger.Auth.Features.PhoneTicket.GetLoginOrRegisterTicket;

public record GetLoginOrRegisterTicketCommandResponse(DateTime NextTry, bool CodeSent, bool IsLogin)
    : RequestPhoneTicketResponse(NextTry, CodeSent);