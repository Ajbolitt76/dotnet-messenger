using Messenger.Auth.Models;
using Messenger.Core.Requests.Abstractions;

namespace Messenger.Auth.Features.PhoneTicket.AcquirePhoneTicket;

public record AcquirePhoneTicketCommand(string Phone, PhoneTicketScopes Scope, string Code) 
    : TicketMetadata(Phone, Scope), ICommand<AcquirePhoneTicketResponse>;
