using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Messenger.Infrastructure.Endpoints;

namespace Messenger.Auth.Features.RefreshToken;

public class RefreshTokenEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
            "/refresh",
            async (RefreshTokenCommand command, IMediator mediatr) => await mediatr.Send(command));
    }
}
