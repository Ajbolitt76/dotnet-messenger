using Messenger.Conversations.PrivateMessages.Extensions;
using Messenger.Core.Constants;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Model.ConversationAggregate.ConversationInfos;
using Messenger.Core.Model.ConversationAggregate.Members;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Requests.Responses;
using Messenger.Core.Services;
using Messenger.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Conversations.PrivateMessages.Features.CreatePrivateConversation;

public class CreatePrivateConversationCommandHandler
    : ICommandHandler<CreatePrivateConversationCommand, CreatedResponse<Guid>>
{
    private readonly IDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreatePrivateConversationCommandHandler(IDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<CreatedResponse<Guid>> Handle(
        CreatePrivateConversationCommand request,
        CancellationToken cancellationToken)
    {
        var initiator = await _dbContext.MessengerUsers.FirstOrNotFoundAsync(
            x => x.Id == request.InitiatorId,
            cancellationToken: cancellationToken);

        var recipient = await _dbContext.MessengerUsers.FirstOrNotFoundAsync(
            x => x.Id == request.ReceiverId,
            cancellationToken: cancellationToken);

        // TODO: Blacklisting

        var existing =
            await _dbContext.PersonalChatInfos.GetConversationBetweenPeersAsync(
                request.ReceiverId,
                request.InitiatorId);

        if (existing != null)
            return new CreatedResponse<Guid>(false, existing.ConversationId);

        var conversation = new Conversation
        {
            Title = $"DM-{recipient.Id}-{initiator.Id}",
            ConversationType = PersonalChatInfo.Discriminator,
            CreatedAt = _dateTimeProvider.NowUtc,
            LastMessageDate = _dateTimeProvider.NowUtc,
            HardDeletedCount = 0
        };

        _dbContext.Conversations.Add(conversation);

        var chatInfo = new PersonalChatInfo
        {
            InitiatorPeer = initiator.Id,
            RecipientPeer = recipient.Id,
            ConversationId = conversation.Id,
        };

        _dbContext.PersonalChatInfos.Add(chatInfo);

        _dbContext.PersonalChatMembers.AddRange(
            new[]
            {
                new PersonalChatMember()
                {
                    ConversationId = conversation.Id,
                    UserId = initiator.Id
                },
                new PersonalChatMember()
                {
                    ConversationId = conversation.Id,
                    UserId = recipient.Id
                }
            });

        _dbContext.ConversationMessages.Add(
            new ConversationMessage()
            {
                ConversationId = conversation.Id,
                TextContent = SystemMessagesTexts.PersonalChatCreated,
                SentAt = _dateTimeProvider.NowUtc,
                Position = 0,
                SenderId = Guid.Empty
            });

        await _dbContext.SaveEntitiesAsync(cancellationToken);

        return new CreatedResponse<Guid>(true, conversation.Id);
    }
}
