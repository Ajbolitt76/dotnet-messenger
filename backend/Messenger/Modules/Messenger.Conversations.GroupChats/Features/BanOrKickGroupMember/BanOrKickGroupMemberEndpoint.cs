using MediatR;
using Messenger.Conversations.GroupChats.Models;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.GroupChats.Features.BanOrKickGroupMember;

public class BanOrKickGroupMemberEndpoint : IEndpoint
{
    public record BanOrKickGroupMemberDto(
        PenaltyScopes Penalty,
        Guid ToUserId,
        Guid ConversationId);
    
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/penalty",
                async (BanOrKickGroupMemberDto dto, IMediator mediator, IUserService userService)
                    => Results.Ok(
                        await mediator.Send(
                            new BanOrKickGroupMemberCommand(
                                dto.Penalty,
                                userService.GetUserIdOrThrow(),
                                dto.ToUserId,
                                dto.ConversationId))))
            .RequireAuthorization()
            .Produces<bool>()
            .WithName("Кикнуть или забанить участника");
    }
}
