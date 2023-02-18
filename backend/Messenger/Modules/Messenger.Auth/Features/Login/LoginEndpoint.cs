using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Messenger.Infrastructure.Endpoints;

namespace Messenger.Auth.Features.Login;

public class LoginEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/login",
                async (LoginCommand command, IMediator mediatr) =>
                {
                    var result = await mediatr.Send(command);
                    return result;
                })
            .WithDescription("Login")
            .ProducesProblem(404)
            .ProducesProblem(401);
    }
}