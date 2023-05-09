using MediatR;
using Messenger.Conversations.GroupChats.Models;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.GroupChats.Features.ExcludeGroupMember;

public class ExcludeGroupMemberEndpoint : IEndpoint
{
    public record ExcludeGroupMemberDto(bool Ban);
    
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "{conversationId:guid}/exclude/{userId:guid}",
                async (
                        Guid conversationId,
                        ExcludeGroupMemberDto dto,
                        Guid userId,
                        IMediator mediator,
                        IUserService userService)
                    => Results.Ok(
                        await mediator.Send(
                            new ExcludeGroupMemberCommand(
                                dto.Ban,
                                userService.GetUserIdOrThrow(),
                                userId,
                                conversationId))))
            .RequireAuthorization()
            .Produces<bool>()
            .WithName("Кикнуть или забанить участника");
    }
}