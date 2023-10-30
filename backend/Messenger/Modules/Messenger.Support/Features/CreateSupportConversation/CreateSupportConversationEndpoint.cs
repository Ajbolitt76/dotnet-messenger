using MediatR;
using Messenger.Conversations.PrivateMessages.Features.CreatePrivateConversation;
using Messenger.Core.Requests.Responses;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Support.Features.CreateSupportConversation;

public class CreateSupportConversationEndpoint : IEndpoint
{
    private readonly Guid _supportId = SupportSetup.SupportId;
    
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/create",
                async (IMediator mediator, IUserService userService)
                    => Results.Ok(
                        new
                        {
                            Id = await mediator.Send(
                                new CreatePrivateConversationCommand(userService.GetUserIdOrThrow(), _supportId))
                        }
                    ))
            .RequireAuthorization()
            .Produces<CreatedResponse<Guid>>()
            .WithName("Создать чат поддержки");
    }
}
