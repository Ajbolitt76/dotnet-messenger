using MediatR;
using Messenger.Infrastructure.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.Features.DeleteMessageCommand;

public class DeleteMessageEndPoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapDelete(
                "/deleteMessage/{messageId:guid}",
                async (Guid messageId, bool deleteFromAll, IMediator mediator)
                    => Results.Ok(
                        await mediator.Send(
                            new DeleteMessageCommand(messageId, deleteFromAll))))
            .RequireAuthorization()
            .WithName("Удалить сообщение из переписки");
    }
}
