using MediatR;
using Messenger.Conversations.GroupChats.Models;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.GroupChats.Features.BanOrKickGroupMember;

public class BanOrKickGroupMemberEndpoint : IEndpoint
{
    public record BanOrKickGroupMemberDto(PenaltyScopes Penalty);
    
    public void Map(IEndpointRouteBuilder endpoints)
    {
        //TODO использовать enum в роуте, чтобы получить conversationId/ban/userId
        endpoints.MapPost(
                "{conversationId:guid}/penalty/{toUserId:guid}",
                async (
                        Guid conversationId,
                        BanOrKickGroupMemberDto dto,
                        Guid toUserId,
                        IMediator mediator,
                        IUserService userService)
                    => Results.Ok(
                        await mediator.Send(
                            new BanOrKickGroupMemberCommand(
                                dto.Penalty,
                                userService.GetUserIdOrThrow(),
                                toUserId,
                                conversationId))))
            .RequireAuthorization()
            .Produces<bool>()
            .WithName("Кикнуть или забанить участника");
    }
}