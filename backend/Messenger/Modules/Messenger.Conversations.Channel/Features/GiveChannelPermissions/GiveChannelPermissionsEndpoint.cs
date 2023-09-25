using MediatR;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.Channel.Features.GiveChannelPermissions;

public class GiveChannelPermissionsEndpoint : IEndpoint
{
    public record GiveChannelPermissionsDto(
        IEnumerable<ChannelMemberPermissions> Permissions,
        bool MakeAdmin);
    
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "{conversationId:guid}/permissions/{userId:guid}",
                async (
                        GiveChannelPermissionsDto dto,
                        Guid userId,
                        Guid conversationId,
                        IMediator mediator,
                        IUserService userService)
                    => Results.Ok(
                        await mediator.Send(
                            new GiveChannelPermissionsCommand(
                                userService.GetUserIdOrThrow(),
                                userId,
                                conversationId,
                                dto.Permissions,
                                dto.MakeAdmin))))
            .RequireAuthorization()
            .Produces<bool>()
            .WithName("Изменить права участника канала");
    }
}
