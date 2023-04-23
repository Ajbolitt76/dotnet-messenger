using MediatR;
using Messenger.Infrastructure.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.Features.GetConversations;

public class GetConversationsEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/conv-list",
            ([FromServices]IMediator mediatr) =>
            {
                return mediatr.Send(new GetConversationsQuery());
            })
            .WithDescription("Получает список переписок")
            .RequireAuthorization();
    }
}
