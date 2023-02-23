using Messenger.Conversations.PrivateMessages.Features.CreatePrivateConversation;
using Messenger.Infrastructure.Endpoints;
using Messenger.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.PrivateMessages;

public class PrivateMessagesRoutes : IEndpointRoot
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGroup("/pms")
            .WithTags("Личные сообщения")
            .AddEndpoint<CreatePrivateConversationEndpoint>();
    }
}
