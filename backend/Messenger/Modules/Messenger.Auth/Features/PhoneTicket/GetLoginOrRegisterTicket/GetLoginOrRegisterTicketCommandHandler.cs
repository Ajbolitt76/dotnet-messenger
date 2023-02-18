using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Messenger.Auth.Features.PhoneTicket.RequestPhoneTicket;
using Messenger.Auth.Models;
using Messenger.Core.Requests.Abstractions;
using Messenger.Infrastructure.User;

namespace Messenger.Auth.Features.PhoneTicket.GetLoginOrRegisterTicket;

public class GetLoginOrRegisterTicketCommandHandler :
    ICommandHandler<GetLoginOrRegisterTicketCommand, GetLoginOrRegisterTicketCommandResponse>
{
    private readonly IDomainHandler<RequestPhoneTicketCommand, RequestPhoneTicketResponse> _ticketHandler;
    private readonly UserManager<ApplicationUser> _userManager;

    public GetLoginOrRegisterTicketCommandHandler(
        IDomainHandler<RequestPhoneTicketCommand, RequestPhoneTicketResponse> ticketHandler,
        UserManager<ApplicationUser> userManager)
    {
        _ticketHandler = ticketHandler;
        _userManager = userManager;
    }

    public async Task<GetLoginOrRegisterTicketCommandResponse> Handle(GetLoginOrRegisterTicketCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await _userManager.Users.AnyAsync(x => x.PhoneNumber == request.Phone);
        
        var ticketScope = exists ? PhoneTicketScopes.LoginTicket : PhoneTicketScopes.RegisterTicket;
        
        var result = await _ticketHandler.Handle(
            new RequestPhoneTicketCommand(
                request.Phone,
                ticketScope),
            cancellationToken);

        return new GetLoginOrRegisterTicketCommandResponse(result.NextTry, result.CodeSent, exists);
    }
}