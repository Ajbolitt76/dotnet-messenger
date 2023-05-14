using MediatR;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.Channel.Features.GetChannelMemberInfo;

public class GetChannelMemberInfoEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/{conversationId:guid}/info/{userId:guid}",
                async (Guid conversationId, Guid userId, IMediator mediator)
                    => Results.Ok(
                        await mediator.Send(
                            new GetChannelMemberInfoQuery(
                                userId,
                                conversationId))))
            .Produces<GetChannelMemberInfoResponse>()
            .WithName("Получить информацию об участнике канала");
    }
}
