using Messenger.Core.Requests.Abstractions;

namespace Messenger.Auth.Features.PhoneTicket.GetLoginOrRegisterTicket;

public record GetLoginOrRegisterTicketCommand(string Phone) : ICommand<GetLoginOrRegisterTicketCommandResponse>;