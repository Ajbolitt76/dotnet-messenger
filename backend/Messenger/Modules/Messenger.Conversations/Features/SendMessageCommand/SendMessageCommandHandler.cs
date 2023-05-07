using Messenger.Conversations.Common.MessageActions.SendMessage;
using Messenger.Conversations.Common.Models;
using Messenger.Conversations.Common.Services;
using Messenger.Core.Exceptions;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Model.ConversationAggregate.Attachment;
using Messenger.Core.Model.SubscriptionAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.Infrastructure.Extensions;
using Messenger.SubscriptionPlans.Enums;
using Messenger.SubscriptionPlans.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Messenger.Conversations.Features.SendMessageCommand;

public partial class SendMessageCommandHandler : ICommandHandler<SendMessageCommand, SendMessageCommandResponse>
{
    private readonly IDbContext _dbContext;
    private readonly IRedisStore<Conversation> _cache;
    private readonly MessageHandlerProvider _messageHandlerProvider;
    private readonly ILogger<SendMessageCommandHandler> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;

    public SendMessageCommandHandler(
        IDbContext dbContext,
        IRedisStore<Conversation> cache,
        MessageHandlerProvider messageHandlerProvider,
        ILogger<SendMessageCommandHandler> logger,
        IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _cache = cache;
        _messageHandlerProvider = messageHandlerProvider;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<SendMessageCommandResponse> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var userSubscription = await _dbContext.UsersSubscriptions.FirstOrDefaultAsync(x => x.UserId == request.UserId && x.ExpiresAt > _dateTimeProvider.NowUtc);
     
        ValidateMessageLength(userSubscription, request.TextContent);
        
        // TODO: Cache / Very hot path
        var conversation = await _dbContext.Conversations.FirstOrNotFoundAsync(x => x.Id == request.ConversationId);

        var handler = _messageHandlerProvider.GetMessageHandler<SendMessageAction, SendMessageActionResponse>(conversation.ConversationType);

        if (handler is null)
        {
            CouldNotFindHandler(conversation.ConversationType);
            throw new NotFoundException<SendMessageAction>();
        }

        var messageData = new MessageData(request.TextContent);

        if (messageData.Attachments != null && messageData.Attachments.Any())
            ValidateMessageAttachmentsCost(userSubscription, messageData.Attachments);
        
        return new(await handler.Handle(
            new SendMessageAction(conversation, messageData),
            cancellationToken));
    }

    private void ValidateMessageLength(UserSubscription? userSubscription, string message)
    {
        if (userSubscription == default)
        {
            if (message.Length > Plans.PlansDict[0].MessageCharsLimit)
                throw new UnauthorizedAccessException($"OVER_{Plans.PlansDict[0].MessageCharsLimit}_MESSAGE_LENGTH_FORBIDDEN");
        }
        else
        {
            var planDetails = Plans.PlansDict[(Plan)userSubscription.Plan];
            if (message.Length > planDetails.MessageCharsLimit)
                throw new UnauthorizedAccessException($"OVER_{Plans.PlansDict[0].MessageCharsLimit}_MESSAGE_LENGTH_FORBIDDEN");
        }
    }

    private void ValidateMessageAttachmentsCost(UserSubscription? userSubscription, IEnumerable<IAttachment> attachments)
    {
        if (userSubscription == default)
        {
            if (attachments.Sum(a => a.Cost) > Plans.PlansDict[0].AttachmentsCostPerMessage)
                throw new UnauthorizedAccessException($"OVER_{Plans.PlansDict[0].AttachmentsCostPerMessage}KB_ATTACHMENTS_FORBIDDEN");
        }
        else
        {
            var planDetails = Plans.PlansDict[(Plan)userSubscription.Plan];
            if (attachments.Sum(a => a.Cost) > planDetails.AttachmentsCostPerMessage)
                throw new UnauthorizedAccessException($"OVER_{planDetails.AttachmentsCostPerMessage}KB_ATTACHMENTS_FORBIDDEN");
        }
    }
    
    [LoggerMessage(
        EventId = 120,
        Level = LogLevel.Error,
        EventName = "SendMessageUnknownHandler",
        Message = "Не удалось найти хэндлер для отправки сообщения {discriminator}")]
    partial void CouldNotFindHandler(string discriminator);
}
