﻿using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Model.ConversationAggregate.Members;
using Messenger.Core.Model.ConversationAggregate.Permissions;

namespace Messenger.Conversations.GroupChats.Extensions;

public static class GroupChatMembersCreation
{
    public static IEnumerable<GroupChatMember> CreateGroupChatMembers(
        this IEnumerable<Guid> userIds,
        Guid conversationId) =>
        userIds.Select(
            id => new GroupChatMember
            {
                ConversationId = conversationId,
                UserId = id,
                WasExcluded = false,
                WasBanned = false,
                IsAdmin = false,
                IsOwner = false,
                Permissions = GroupChatPermissionPresets.NewMember
            });

    public static IEnumerable<ConversationUserStatus> CreateConversationUserStatuses(
        this IEnumerable<Guid> userIds,
        Guid conversationId) =>
        userIds
            .Select(
                id => new ConversationUserStatus
                {
                    ConversationId = conversationId,
                    UserId = id,
                    ReadTo = -1,
                    DeletedTo = null,
                    SoftDeletedCount = 0,
                    IsDeletedByUser = false
                });
}
