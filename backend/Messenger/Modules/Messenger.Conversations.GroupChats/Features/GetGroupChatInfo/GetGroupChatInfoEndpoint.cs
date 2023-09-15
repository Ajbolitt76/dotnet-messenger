using MediatR;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Models;

namespace Messenger.Conversations.GroupChats.Features.GetGroupChatInfo;

public class GetGroupChatInfoEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/{conversationId:guid}/info",
                async (Guid conversationId, IMediator mediator, IUserService userService)
                    => Results.Ok(
                        await mediator.Send(
                            new GetGroupChatInfoQuery(userService.GetUserIdOrThrow(), conversationId))))
            .RequireAuthorization()
            .WithOpenApi()
            .WithSummary("Получить информацию о чате")
            .WithDescription("description")
            .Produces<GetGroupChatInfoQueryResponse>();
    }
}
