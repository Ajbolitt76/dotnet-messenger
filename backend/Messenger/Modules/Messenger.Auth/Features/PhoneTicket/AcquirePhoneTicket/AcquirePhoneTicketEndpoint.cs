using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Messenger.Infrastructure.Endpoints;

namespace Messenger.Auth.Features.PhoneTicket.AcquirePhoneTicket;

public class AcquirePhoneTicketEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/verify-phone-ticket", async (AcquirePhoneTicketCommand command, IMediator mediatr) 
            => await mediatr.Send(command));
    }
}