using MediatR;
using Messenger.Infrastructure.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.Features.DeleteMessageCommand;

public class DeleteMessageEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                "/{conversationId:guid}/message/{messageId:guid}",
                async (Guid conversationId, Guid messageId, bool deleteFromAll, IMediator mediator)
                    => Results.Ok(
                        await mediator.Send(
                            new DeleteMessageCommand(conversationId, messageId, deleteFromAll))))
            .RequireAuthorization()
            .WithName("Удалить сообщение из переписки");
    }
}
