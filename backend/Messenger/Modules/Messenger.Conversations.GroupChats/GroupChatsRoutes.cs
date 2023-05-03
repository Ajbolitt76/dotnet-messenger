using Messenger.Conversations.GroupChats.Features.ChangeGroupChatInfo;
using Messenger.Conversations.GroupChats.Features.CreateGroupChat;
using Messenger.Conversations.GroupChats.Features.ExcludeGroupMember;
using Messenger.Conversations.GroupChats.Features.GetGroupChatInfo;
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
            .AddEndpoint<GetGroupChatInfoEndpoint>()
            .AddEndpoint<CreateGroupChatEndpoint>()
            .AddEndpoint<InviteToGroupChatEndpoint>()
            .AddEndpoint<ExcludeGroupMemberEndpoint>()
            .AddEndpoint<GivePermissionsEndpoint>()
            .AddEndpoint<MuteGroupMemberEndpoint>()
            .AddEndpoint<ChangeGroupInfoEndpoint>();
    }
}
