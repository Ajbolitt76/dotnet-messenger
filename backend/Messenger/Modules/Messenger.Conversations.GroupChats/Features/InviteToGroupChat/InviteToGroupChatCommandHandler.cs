using Messenger.Conversations.GroupChats.Extensions;
using Messenger.Conversations.GroupChats.Models;
using Messenger.Core.Constants;
using Messenger.Core.Model.ConversationAggregate.ConversationInfos;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Requests.Abstractions;
using Messenger.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Conversations.GroupChats.Features.InviteToGroupChat;

public class InviteToGroupChatCommandHandler : ICommandHandler<InviteToGroupChatCommand, NotAddedUsers>
{
    private readonly IDbContext _dbContext;

    public InviteToGroupChatCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<NotAddedUsers> Handle(InviteToGroupChatCommand request, CancellationToken cancellationToken)
    {
        var conversation = await _dbContext.Conversations.FirstOrNotFoundAsync(
            conversation => conversation.Id == request.ConversationId
                            && conversation.ConversationType == GroupChatInfo.Discriminator,
            cancellationToken);

        (await _dbContext.GroupChatMembers.GetGroupMemberOrThrowAsync(request.InviterId, conversation.Id))
            .CheckForBanOrExcludeAndThrow()
            .CheckForPermissionsAndThrow(GroupMemberPermissions.InviteMembers);

        var (newMembers, notAdded) =
            await GetNewIdsAndChangeExcludeMembers(request.InvitedMembers.ToList(), conversation.Id);

        var groupMembers = newMembers.CreateGroupChatMembers(conversation.Id);

        var userStatuses = newMembers.CreateConversationUserStatuses(conversation.Id);
        
        _dbContext.GroupChatMembers.AddRange(groupMembers);
        _dbContext.ConversationUserStatuses.AddRange(userStatuses);
        await _dbContext.SaveEntitiesAsync(cancellationToken);

        return notAdded;
    }

    private async Task<(List<Guid>, NotAddedUsers)> GetNewIdsAndChangeExcludeMembers(List<Guid> ids, Guid conversationId)
    {
        var groupMembers = await _dbContext.GroupChatMembers
            .Where(member => member.ConversationId == conversationId && ids.Contains(member.UserId))
            .ToListAsync();

        var groupMembersId = groupMembers.Select(member => member.UserId).ToList();

        var newUsersId = await _dbContext.MessengerUsers
            .Where(user => ids.Contains(user.Id) && !groupMembersId.Contains(user.Id))
            .Select(user => user.Id)
            .ToListAsync();

        var notAddedIds = new NotAddedUsers(
            ids.Except(newUsersId).Except(groupMembersId),
            InviteProblemMessages.NotFound);

        foreach (var member in groupMembers)
        {
            switch (member)
            {
                case {WasBanned: true}:
                    notAddedIds.Add(member.Id, InviteProblemMessages.Banned);
                    break;
                case { WasExcluded: true}:
                    member.WasExcluded = false;
                    break;
                default:
                    notAddedIds.Add(member.Id, InviteProblemMessages.ChatMember);
                    break;
            }
        }
        return (newUsersId, notAddedIds);
    }
}
