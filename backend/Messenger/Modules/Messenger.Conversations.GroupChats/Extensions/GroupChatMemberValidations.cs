using Messenger.Core.Exceptions;
using Messenger.Core.Model.ConversationAggregate.Members;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Conversations.GroupChats.Extensions;

public static class GroupChatMemberValidations
{
    public static async Task<GroupChatMember> GetGroupMemberOrThrowAsync(
        this IQueryable<GroupChatMember> queryable,
        Guid userId,
        Guid conversationId) =>
        await queryable
            .FirstOrDefaultAsync(member => member.UserId == userId && member.ConversationId == conversationId)
        ?? throw new ForbiddenException("Not a chat member");

    public static GroupChatMember CheckForBanOrExcludeAndThrow(this GroupChatMember member) =>
        member.WasBanned ? throw new ForbiddenException("Was banned") :
        member.WasExcluded ? throw new ForbiddenException("Was excluded") :
        member;

    public static GroupChatMember CheckForMuteAndThrow(this GroupChatMember member, DateTime utcNow) => 
        member.MutedTill > utcNow ? throw new ForbiddenException("Muted") : member;

    public static GroupChatMember CheckForPermissionsAndThrow(
        this GroupChatMember member,
        GroupMemberPermissions requiredPermissions) =>
        (member.Permissions & requiredPermissions) != requiredPermissions
            ? throw new ForbiddenException("Not enough permissions")
            : member;
}
