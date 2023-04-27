using Messenger.Conversations.GroupChats.Extensions;
using Messenger.Core.Exceptions;
using Messenger.Core.Model.ConversationAggregate.ConversationInfos;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Requests.Abstractions;
using Messenger.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Conversations.GroupChats.Features.InviteToGroupChat;

public class InviteToGroupChatCommandHandler : ICommandHandler<InviteToGroupChatCommand, bool>
{
    private readonly IDbContext _dbContext;

    public InviteToGroupChatCommandHandler(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(InviteToGroupChatCommand request, CancellationToken cancellationToken)
    {
        var conversation = await _dbContext.Conversations.FirstOrNotFoundAsync(
            conversation => conversation.Id == request.ChatId
                            && conversation.ConversationType == GroupChatInfo.Discriminator,
            cancellationToken);
        
        var currentUser = await _dbContext.GroupChatMembers
            .GetGroupMemberOrThrowAsync(request.InviterId, conversation.Id);
        currentUser
            .CheckForBanOrExcludeAndThrow()
            .CheckForPermissionsAndThrow(GroupMemberPermissions.InviteMembers);

        var newMembers = await InviteNewAndChangeStatusOldMembers(request.InvitedMembers, conversation.Id);

        var groupMembers = newMembers.SelectNewGroupChatMembers(conversation.Id);

        var userStatuses = newMembers.SelectConversationUserStatuses(conversation.Id);
        
        _dbContext.GroupChatMembers.AddRange(groupMembers);
        _dbContext.ConversationUserStatuses.AddRange(userStatuses);
        await _dbContext.SaveEntitiesAsync(cancellationToken);

        return true;//TODO подумать во что можно переделать
    }

    private async Task<List<Guid>> InviteNewAndChangeStatusOldMembers(IEnumerable<Guid> ids, Guid conversationId)
    {
        var newMembers = new List<Guid>();
        
        foreach (var userId in ids)
        {
            await _dbContext.MessengerUsers.AnyOrNotFoundAsync(
                user => user.Id == userId);

            var member = await _dbContext.GroupChatMembers.FirstOrDefaultAsync(
                x => x.ConversationId == conversationId && x.UserId == userId);
            switch (member)
            {
                case null:
                    newMembers.Add(userId);
                    continue;
                case {WasBanned: true}:
                    throw new ForbiddenException("User is banned");
                //TODO подумать какую кидать ошибку
                case {WasExcluded: true}:
                    member.WasExcluded = false;
                    continue;
                //Пользователь является действующим участником чата
                default:
                    continue;
            }
        }
        return newMembers;
    }
}
