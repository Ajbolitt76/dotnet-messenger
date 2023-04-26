using Messenger.Core.Constants;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Model.ConversationAggregate.ConversationInfos;
using Messenger.Core.Model.ConversationAggregate.Members;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Model.UserAggregate;
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
                  x => x.Id == request.InitiatorId,
                  cancellationToken: cancellationToken);
        
        var members = new List<MessengerUser>();
    
        foreach (var userId in request.InvitedMembers)
        { 
            var member = await _dbContext.MessengerUsers.FirstOrNotFoundAsync(
              x => x.Id == userId, 
              cancellationToken: cancellationToken); 
            members.Add(member);
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

        var groupMembers = members.Select(
            user => new GroupChatMember
            {
                ConversationId = conversation.Id,
                UserId = user.Id,
                WasExcluded = false,
                WasBanned = false,
                IsAdmin = false,
                IsOwner = false,
                Permissions = GroupChatPermissionPresets.NewMember
            });

        var conversationStatuses = members
            .Append(initiator)
            .Select(
                user => new ConversationUserStatus
                {
                    ConversationId = conversation.Id,
                    UserId = user.Id,
                    ReadTo = -1,
                    DeletedTo = null,
                    SoftDeletedCount = 0,
                    IsDeletedByUser = false
                });
        var system = await _dbContext.MessengerUsers
            .FirstOrNotFoundAsync(x => x.IdentityUserId == Guid.Empty, cancellationToken);
        _dbContext.ConversationMessages.Add(
            new ConversationMessage()
            {
                ConversationId = conversation.Id,
                TextContent = SystemMessagesTexts.GroupChatCreated,
                SentAt = _dateTimeProvider.NowUtc,
                Position = 0,
                SenderId = system.Id
            });

        _dbContext.GroupChatMembers.AddRange(groupMembers.Append(initiatorMember));
        _dbContext.ConversationUserStatuses.AddRange(conversationStatuses);
        await _dbContext.SaveEntitiesAsync(cancellationToken);
        
        return new CreatedResponse<Guid>(true, conversation.Id);
    }
}
