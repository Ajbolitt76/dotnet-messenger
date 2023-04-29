using Messenger.Conversations.Common.Abstractions;
using Messenger.Conversations.Common.MessageActions;
using Messenger.Conversations.GroupChats.Extensions;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.Infrastructure.Extensions;

namespace Messenger.Conversations.GroupChats.MessageActions.GroupDeleteMessage;

public class GroupDeleteMessageActionHandler : IMessageActionHandler<DeleteMessageAction, bool>
{
    private readonly IDbContext _dbContext;
    private readonly IUserService _userService;

    public GroupDeleteMessageActionHandler(IDbContext dbContext, IUserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    public static string MessageType => ConversationTypes.GroupChat;
    
    public async Task<bool> Handle(DeleteMessageAction request, CancellationToken cancellationToken)
    {
        var message = await _dbContext.ConversationMessages.FirstOrNotFoundAsync(
            message => message.Id == request.MessageId && message.ConversationId == request.ConversationId,
            cancellationToken: cancellationToken);
        var currentUserId = _userService.GetUserIdOrThrow();
        var member =
            (await _dbContext.GroupChatMembers.GetGroupMemberOrThrowAsync(currentUserId, request.ConversationId))
            .CheckForBanOrExcludeAndThrow();
        
        if (request.DeleteFromAll && 
            (currentUserId == message.SenderId || (member.Permissions & GroupMemberPermissions.DeleteMessages) != 0))
        {
            await DecreaseSoftDeletedCount(message.DeletedFrom, request.ConversationId);
            
            _dbContext.ConversationMessages.Remove(message);
            
            request.Conversation.HardDeletedCount++;
        }
        else
        {
            var conversationStatus = await _dbContext.ConversationUserStatuses.Where(
                    status => status.UserId == currentUserId && status.ConversationId == request.ConversationId)
                .FirstOrNotFoundAsync(cancellationToken: cancellationToken);

            conversationStatus.SoftDeletedCount++;
            
            message.DeletedFrom ??= new List<Guid>();
            if (!message.DeletedFrom.Contains(currentUserId))
                message.DeletedFrom.Add(currentUserId);
        }

        await _dbContext.SaveEntitiesAsync(cancellationToken);

        return true;
    }

    private async Task DecreaseSoftDeletedCount(List<Guid>? userIds, Guid conversationId)
    {
        if (userIds == null)
            return;

        foreach (var userId in userIds)
        {
            var status = await _dbContext.ConversationUserStatuses
                .FirstOrNotFoundAsync(status => status.UserId == userId && status.ConversationId == conversationId);

            status.SoftDeletedCount--;
        }
    }
}
