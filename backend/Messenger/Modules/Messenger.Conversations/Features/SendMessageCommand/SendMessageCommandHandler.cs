using Messenger.Conversations.Common.MessageActions.SendMessage;
using Messenger.Conversations.Common.Models;
using Messenger.Conversations.Common.Services;
using Messenger.Core.Exceptions;
using Messenger.Core.Model.ConversationAggregate;
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
        await ValidateMessageLength(request.UserId, request.TextContent);
        
        // TODO: Cache / Very hot path
        var conversation = await _dbContext.Conversations.FirstOrNotFoundAsync(x => x.Id == request.ConversationId);

        var handler = _messageHandlerProvider.GetMessageHandler<SendMessageAction, SendMessageActionResponse>(conversation.ConversationType);

        if (handler is null)
        {
            CouldNotFindHandler(conversation.ConversationType);
            throw new NotFoundException<SendMessageAction>();
        }
        
        return new(await handler.Handle(
            new SendMessageAction(conversation, new MessageData(request.TextContent)),
            cancellationToken));
    }
    
    private async Task ValidateMessageLength(Guid userId, string message)
    {
        var userSubscription = await _dbContext.UsersSubscriptions.FirstOrDefaultAsync(x => x.UserId == userId && x.ExpiresAt > _dateTimeProvider.NowUtc);
        if (userSubscription == default)
        {
            if (message.Length > 100)
                throw new UnauthorizedAccessException("TO_LONG_MESSAGE");
        }
        else
        {
            var planDetails = Plans.PlansDict[(Plan)userSubscription.Plan];
            if (message.Length > planDetails.MessageCharsLimit)
                throw new UnauthorizedAccessException("TO_LONG_MESSAGE");
        }
    }
    
    [LoggerMessage(
        EventId = 120,
        Level = LogLevel.Error,
        EventName = "SendMessageUnknownHandler",
        Message = "Не удалось найти хэндлер для отправки сообщения {discriminator}")]
    partial void CouldNotFindHandler(string discriminator);
}
