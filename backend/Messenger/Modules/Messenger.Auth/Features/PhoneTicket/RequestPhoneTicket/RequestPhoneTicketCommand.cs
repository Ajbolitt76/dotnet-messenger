using Messenger.Auth.Models;
using Messenger.Core.Requests.Abstractions;

namespace Messenger.Auth.Features.PhoneTicket.RequestPhoneTicket;

public record RequestPhoneTicketCommand(string Phone, PhoneTicketScopes Scope)
    : TicketMetadata(Phone, Scope), ICommand<RequestPhoneTicketResponse>;