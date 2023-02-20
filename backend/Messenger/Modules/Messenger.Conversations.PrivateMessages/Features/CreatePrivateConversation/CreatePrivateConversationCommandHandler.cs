using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Model.ConversationAggregate.ConversationInfos;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Conversations.PrivateMessages.Features.CreatePrivateConversation;

public class CreatePrivateConversationCommandHandler : ICommandHandler<CreatePrivateConversationCommand, Guid>
{
    private readonly IDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreatePrivateConversationCommandHandler(IDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Guid> Handle(CreatePrivateConversationCommand request, CancellationToken cancellationToken)
    {
        var initiator = await _dbContext.MessengerUsers.FirstOrNotFound(
            x => x.Id == request.InitiatorId,
            cancellationToken: cancellationToken);
        
        var recipient = await _dbContext.MessengerUsers.FirstOrNotFound(
            x => x.Id == request.InitiatorId,
            cancellationToken: cancellationToken);

        // TODO: Blacklist

        var conversation = new Conversation
        {
            Title = "",
            ConversationType = PersonalChatInfo.Discriminator,
            CreatedAt = _dateTimeProvider.NowUtc,
            LastMessage = _dateTimeProvider.NowUtc
        };

        _dbContext.Conversations.Add(conversation);
        
        var chatInfo = new PersonalChatInfo
        {
            InitiatorPeer = initiator.Id,
            RecipientPeer = recipient.Id,
            ConversationId = conversation.Id
        };

        _dbContext.PersonalChatInfos.Add(chatInfo);
        await _dbContext.SaveEntitiesAsync(cancellationToken);

        return conversation.Id;
    }
}
