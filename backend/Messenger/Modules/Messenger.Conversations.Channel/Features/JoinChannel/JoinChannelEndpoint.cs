using MediatR;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.Channel.Features.JoinChannel;

public class JoinChannelEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "{conversationId:guid}/join",
                async (Guid conversationId, IMediator mediator, IUserService userService) =>
                    Results.Ok(
                        await mediator.Send(
                            new JoinChannelCommand(
                                userService.GetUserIdOrThrow(),
                                conversationId))))
            .RequireAuthorization()
            .Produces<bool>()
            .WithName("Присоединиться к каналу");
    }
}
