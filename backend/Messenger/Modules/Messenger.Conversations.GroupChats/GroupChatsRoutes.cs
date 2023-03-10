using Messenger.Conversations.GroupChats.Features.CreateGroupChat;
using Messenger.Infrastructure.Endpoints;
using Messenger.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.GroupChats;

public class GroupChatsRoutes : IEndpointRoot
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGroup("/group-chat")
            .WithTags("Групповые чаты")
            .AddEndpoint<CreateGroupChatEndpoint>();
    }
}
