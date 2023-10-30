using MassTransit;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.Rabbit.Contracts;


namespace Messenger.Support.Api.MessageActions.SupportStoreMessage;

public class SupportStoreMessageActionHandler : ISupportStoreMessageActionHandler
{
    private readonly IDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IPublishEndpoint _bus;

    public SupportStoreMessageActionHandler(
        IDbContext dbContext,
        IDateTimeProvider dateTimeProvider,
        IPublishEndpoint bus)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
        _bus = bus;
    }
    
    public async Task<bool> Handle(SupportStoreMessageRequest request, CancellationToken cancellationToken)
    {
        var conversationMessage = new ConversationMessage()
        {
            Attachments = null,
            SentAt = _dateTimeProvider.NowUtc,
            ConversationId = request.ConversationId,
            TextContent = request.Content, 
            SenderId = request.UserId,
            Position = request.MessagePosition,
        };

        _dbContext.ConversationMessages.Add(conversationMessage);
        await _dbContext.SaveEntitiesAsync(cancellationToken);
        
        await _bus.Publish(
            new SupportSendMessageRequest(request.ConversationId, conversationMessage),
            cancellationToken);

        return true;
    }
}
