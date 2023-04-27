using Messenger.Conversations.GroupChats.Extensions;
using Messenger.Core.Constants;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Model.ConversationAggregate.ConversationInfos;
using Messenger.Core.Model.ConversationAggregate.Members;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Requests.Responses;
using Messenger.Core.Services;
using Messenger.Infrastructure.Extensions;

namespace Messenger.Conversations.GroupChats.Features.CreateGroupChat;

public class CreateGroupChatCommandHandler : ICommandHandler<CreateGroupChatCommand, CreatedResponse<Guid>>
{
    private readonly IDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateGroupChatCommandHandler(IDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<CreatedResponse<Guid>> Handle(
        CreateGroupChatCommand request,
        CancellationToken cancellationToken)
    {
        var initiator = await _dbContext.MessengerUsers.FirstOrNotFoundAsync(
                  user => user.Id == request.InitiatorId,
                  cancellationToken: cancellationToken);
        
        var members = new List<Guid>();
    
        foreach (var userId in request.InvitedMembers.Distinct().Where(x => x != initiator.Id))
        {
            await _dbContext.MessengerUsers.AnyOrNotFoundAsync(
                x => x.Id == userId,
                cancellationToken: cancellationToken); 
            members.Add(userId);
        }

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

        var groupMembers = members.SelectNewGroupChatMembers(conversation.Id);
        var userStatuses = members.Append(initiatorMember.UserId)
            .SelectConversationUserStatuses(conversation.Id);
        
        var system = await _dbContext.MessengerUsers
            .FirstOrNotFoundAsync(x => x.IdentityUserId == Guid.Empty, cancellationToken);
        _dbContext.ConversationMessages.Add(
            new ConversationMessage
            {
                ConversationId = conversation.Id,
                TextContent = SystemMessagesTexts.GroupChatCreated,
                SentAt = _dateTimeProvider.NowUtc,
                Position = 0,
                SenderId = system.Id
            });

        _dbContext.GroupChatMembers.AddRange(groupMembers.Append(initiatorMember));
        _dbContext.ConversationUserStatuses.AddRange(userStatuses);
        await _dbContext.SaveEntitiesAsync(cancellationToken);
        
        return new CreatedResponse<Guid>(true, conversation.Id);
    }
}
