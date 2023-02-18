using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Messenger.Infrastructure.Endpoints;

namespace Messenger.Auth.Features.PhoneTicket.GetLoginOrRegisterTicket;

public class GetLoginOrRegisterTicketEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/initiate-phone-auth",
                async (GetLoginOrRegisterTicketCommand command, IMediator mediatr)
                    => await mediatr.Send(command))
            .WithDescription(
                @"Инициировать процесс авторизации по номеру телефона.
В зависимости от того, зарегистрирован ли пользователь с таким номером, 
будет возвращена либо скоуп авторизации либо скоуп регистрации.");
    }
}