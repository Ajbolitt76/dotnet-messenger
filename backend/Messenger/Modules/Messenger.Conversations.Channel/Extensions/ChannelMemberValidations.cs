using Messenger.Core;
using Messenger.Core.Exceptions;
using Messenger.Core.Model.ConversationAggregate.Members;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Conversations.Channel.Extensions;

public static class ChannelMemberValidations
{
    public static async Task<ChannelMember> GetChannelMemberOrThrowAsync(
        this IQueryable<ChannelMember> queryable,
        Guid userId,
        Guid conversationId) =>
        await queryable
            .FirstOrDefaultAsync(member => member.UserId == userId && member.ConversationId == conversationId)
        ?? throw new ForbiddenException(ForbiddenErrorCodes.NotChatMember);

    public static ChannelMember CheckForPermissionsAndThrow(
        this ChannelMember member,
        ChannelMemberPermissions requiredPermissions) =>
        (member.Permissions & requiredPermissions) != requiredPermissions
            ? throw new ForbiddenException(ForbiddenErrorCodes.NotEnoughPermissions)
            : member;
}
