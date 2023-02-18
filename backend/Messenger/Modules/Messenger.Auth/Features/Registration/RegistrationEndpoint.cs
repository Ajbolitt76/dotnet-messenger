using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Messenger.Infrastructure.Endpoints;

namespace Messenger.Auth.Features.Registration;

public class RegistrationEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/registration", async (RegisterUserCommand command, IMediator mediatr) =>
        {
            var result = await mediatr.Send(command);
            return result;
        }).WithDescription("Регистрация");
    }
}