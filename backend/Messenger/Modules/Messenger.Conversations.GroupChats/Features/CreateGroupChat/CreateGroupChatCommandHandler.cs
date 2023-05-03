using Messenger.Conversations.GroupChats.Extensions;
using Messenger.Conversations.GroupChats.Models;
using Messenger.Core.Constants;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Model.ConversationAggregate.ConversationInfos;
using Messenger.Core.Model.ConversationAggregate.Members;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Requests.Responses;
using Messenger.Core.Services;
using Messenger.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Conversations.GroupChats.Features.CreateGroupChat;

public class CreateGroupChatCommandHandler : ICommandHandler<CreateGroupChatCommand, GroupCreatedResponse>
{
    private readonly IDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateGroupChatCommandHandler(IDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<GroupCreatedResponse> Handle(
        CreateGroupChatCommand request,
        CancellationToken cancellationToken)
    {
        var initiator = await _dbContext.MessengerUsers.FirstOrNotFoundAsync(
                  user => user.Id == request.InitiatorId,
                  cancellationToken: cancellationToken);
        
        var (members, notAdded) = await GetInvitedMembers(request.InvitedMemberIds, request.InitiatorId);
        // TODO: Blacklisting

        var conversation = new Conversation
        {
            Title = request.Name, 
            ConversationType = GroupChatInfo.Discriminator,
            CreatedAt = _dateTimeProvider.NowUtc, 
            LastMessageDate = _dateTimeProvider.NowUtc,
            HardDeletedCount = 0
        };

        _dbContext.Conversations.Add(conversation);

        var chatInfo = new GroupChatInfo
        {
            ConversationId = conversation.Id
        };

        _dbContext.GroupChatInfos.Add(chatInfo);

        var initiatorMember = new GroupChatMember
        {
            ConversationId = conversation.Id,
            UserId = initiator.Id,
            WasExcluded = false,
            WasBanned = false,
            IsAdmin = true,
            IsOwner = true,
            Permissions = GroupChatPermissionPresets.Creator
        };

        var groupMembers = members.CreateGroupChatMembers(conversation.Id);
        var userStatuses = members
            .Append(initiatorMember.UserId)
            .CreateConversationUserStatuses(conversation.Id);
        
        _dbContext.ConversationMessages.Add(
            new ConversationMessage
            {
                ConversationId = conversation.Id,
                TextContent = SystemMessagesTexts.GroupChatCreated,
                SentAt = _dateTimeProvider.NowUtc,
                Position = 0,
                SenderId = Guid.Empty
            });

        _dbContext.GroupChatMembers.AddRange(groupMembers.Append(initiatorMember));
        _dbContext.ConversationUserStatuses.AddRange(userStatuses);
        await _dbContext.SaveEntitiesAsync(cancellationToken);
        
        return new GroupCreatedResponse(conversation.Id, notAdded);
    }

    private async Task<(List<Guid>, NotAddedUsers)> GetInvitedMembers(IEnumerable<Guid> invitedIds, Guid initiatorId)
    {
        var clearIds = invitedIds.Distinct().Where(x => x != initiatorId).ToList();

        var foundIds = await _dbContext.MessengerUsers
            .Where(user => clearIds.Contains(user.Id))
            .Select(user => user.Id)
            .ToListAsync();

        var notFoundIds = new NotAddedUsers(clearIds.Except(foundIds), InviteProblemMessages.NotFound);

        return (foundIds, notFoundIds);
    }
}
