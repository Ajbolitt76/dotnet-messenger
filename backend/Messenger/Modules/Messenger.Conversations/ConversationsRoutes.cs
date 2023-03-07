using Messenger.Conversations.Features.DeleteMessageCommand;
using Messenger.Conversations.Features.GetMessages;
using Messenger.Conversations.Features.SendMessageCommand;
using Messenger.Infrastructure.Endpoints;
using Messenger.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations;

public class ConversationsRoutes : IEndpointRoot
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGroup("/c")
            .WithTags("Сообщения")
            .AddEndpoint<SendMessageEndpoint>()
            .AddEndpoint<GetMessagesEndpoint>()
            .AddEndpoint<DeleteMessageEndPoint>();
    }
}
