using FluentValidation;
using MediatR;
using Messenger.Core.Requests.Responses;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Messenger.Infrastructure.Validation.ValidationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.PrivateMessages.Features.CreatePrivateConversation;

public class CreatePrivateConversationEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/create",
                async ([FromQuery] Guid receiverId, IMediator mediator, IUserService userService)
                    => Results.Ok(
                        new
                        {
                            Id = await mediator.Send(
                                new CreatePrivateConversationCommand(userService.GetUserIdOrThrow(), receiverId))
                        }
                    ))
            .RequireAuthorization()
            .Produces<CreatedResponse<Guid>>()
            .WithName("Создать приватный чат");
    }
}
