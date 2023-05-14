using MediatR;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.Channel.Features.LeaveChannel;

public class LeaveChannelEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                "{conversationId:guid}/leave",
                async (Guid conversationId, IMediator mediator, IUserService userService) =>
                    Results.Ok(
                        await mediator.Send(
                            new LeaveChannelCommand(
                                userService.GetUserIdOrThrow(),
                                conversationId))))
            .RequireAuthorization()
            .Produces<bool>()
            .WithName("Выйти из канала");
    }
}
