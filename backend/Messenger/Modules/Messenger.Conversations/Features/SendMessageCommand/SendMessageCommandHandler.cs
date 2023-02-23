using Messenger.Conversations.Common.Abstractions;
using Messenger.Conversations.Common.MessageActions;
using Messenger.Conversations.Common.Models;
using Messenger.Conversations.Common.Services;
using Messenger.Core.Exceptions;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

namespace Messenger.Conversations.Features.SendMessageCommand;

public partial class SendMessageCommandHandler : ICommandHandler<SendMessageCommand, bool>
{
    private readonly IDbContext _dbContext;
    private readonly IRedisStore<Conversation> _cache;
    private readonly MessageHandlerProvider _messageHandlerProvider;
    private readonly ILogger<SendMessageCommandHandler> _logger;

    public SendMessageCommandHandler(
        IDbContext dbContext,
        IRedisStore<Conversation> cache,
        MessageHandlerProvider messageHandlerProvider,
        ILogger<SendMessageCommandHandler> logger)
    {
        _dbContext = dbContext;
        _cache = cache;
        _messageHandlerProvider = messageHandlerProvider;
        _logger = logger;
    }

    public async Task<bool> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        // TODO: Cache / Very hot path
        var conversation = await _dbContext.Conversations.FirstOrNotFoundAsync(x => x.Id == request.ConversationId);

        var handler = _messageHandlerProvider.GetMessageHandler<SendMessageAction, bool>(conversation.ConversationType);

        if (handler is null)
        {
            CouldNotFindHandler(conversation.ConversationType);
            throw new NotFoundException<SendMessageAction>();
        }
        
        return await handler.Handle(
            new SendMessageAction(conversation, new MessageData(request.TextContent)),
            cancellationToken);
    }

    [LoggerMessage(
        EventId = 120,
        Level = LogLevel.Error,
        EventName = "SendMessageUnknownHandler",
        Message = "Не удалось найти хэндлер для отправки сообщения {discriminator}")]
    partial void CouldNotFindHandler(string discriminator);
}
