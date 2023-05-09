using MediatR;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.GroupChats.Features.UnbanGroupMember;

public class UnbanGroupMemberEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "{conversationId:guid}/unban/{userId:guid}",
                async (Guid userId, Guid conversationId, IMediator mediator, IUserService userService)
                    => Results.Ok(
                        await mediator.Send(
                            new UnbanGroupMemberCommand(
                                userService.GetUserIdOrThrow(),
                                userId,
                                conversationId))))
            .RequireAuthorization()
            .Produces<bool>()
            .WithName("Разбанить участника");
    }
}
