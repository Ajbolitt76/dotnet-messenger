using MediatR;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.Channel.Features.GetChannelInfo;

public class GetChannelInfoEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/{conversationId:guid}/info",
                async (Guid conversationId, IMediator mediator)
                    => Results.Ok(
                        await mediator.Send(
                            new GetChannelInfoQuery(conversationId))))
            .Produces<GetChannelInfoQueryResponse>()
            .WithName("Получить информацию о канале");
    }
}
