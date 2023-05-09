using MediatR;
using Messenger.Conversations.GroupChats.Features.GetGroupChatInfo;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.GroupChats.Features.GetGroupChatMemberInfo;

public class GetGroupChatMemberInfoEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/{conversationId:guid}/info/{userId:guid}",
                async (Guid conversationId, Guid userId, IMediator mediator, IUserService userService)
                    => Results.Ok(
                        await mediator.Send(
                            new GetGroupChatMemberInfoQuery(
                                userService.GetUserIdOrThrow(),
                                userId,
                                conversationId))))
            .RequireAuthorization()
            .Produces<GetGroupChatMemberInfoResponse>()
            .WithName("Получить информацию об участнике");
    }
}
