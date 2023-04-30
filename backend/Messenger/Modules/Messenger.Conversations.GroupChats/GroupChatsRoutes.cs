using Messenger.Conversations.GroupChats.Features.BanOrKickGroupMember;
using Messenger.Conversations.GroupChats.Features.CreateGroupChat;
using Messenger.Conversations.GroupChats.Features.GivePermissions;
using Messenger.Conversations.GroupChats.Features.InviteToGroupChat;
using Messenger.Conversations.GroupChats.Features.MuteGroupMember;
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
            .AddEndpoint<CreateGroupChatEndpoint>()
            .AddEndpoint<InviteToGroupChatEndpoint>()
            .AddEndpoint<BanOrKickGroupMemberEndpoint>()
            .AddEndpoint<GivePermissionsEndpoint>()
            .AddEndpoint<MuteGroupMemberEndpoint>();
    }
}
