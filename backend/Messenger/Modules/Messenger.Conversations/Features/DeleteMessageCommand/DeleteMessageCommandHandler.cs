using Messenger.Conversations.Common.MessageActions;
using Messenger.Conversations.Common.Services;
using Messenger.Conversations.Features.SendMessageCommand;
using Messenger.Core.Exceptions;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.Infrastructure.Extensions;
using Messenger.Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace Messenger.Conversations.Features.DeleteMessageCommand;

public partial class DeleteMessageCommandHandler : ICommandHandler<DeleteMessageCommand, bool>
{
    private readonly IDbContext _dbContext;
    private readonly MessageHandlerProvider _messageHandlerProvider;
    private readonly ILogger<SendMessageCommandHandler> _logger;
    
    public DeleteMessageCommandHandler(
        IDbContext dbContext,
        IRedisStore<Conversation> cache,
        MessageHandlerProvider messageHandlerProvider,
        ILogger<SendMessageCommandHandler> logger)
    {
        _dbContext = dbContext;
        _messageHandlerProvider = messageHandlerProvider;
        _logger = logger;
    }

    
    public async Task<bool> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
    {
        var conversation = await _dbContext.Conversations.FirstOrNotFoundAsync(x => x.Id == request.ConversationId); 
        
        var handler = _messageHandlerProvider.GetMessageHandler<DeleteMessageAction, bool>(conversation.ConversationType);
        
        if (handler is null)
        {
            CouldNotFindHandler(conversation.ConversationType);
            throw new NotFoundException<DeleteMessageAction>();
        }
        
        return await handler.Handle(
            new DeleteMessageAction(request.ConversationId, request.MessageId, request.DeleteFromAll, conversation),
            cancellationToken);
    }
    [LoggerMessage(
        EventId = 120,
        Level = LogLevel.Error,
        EventName = "DeleteMessageUnknownHandler",
        Message = "Не удалось найти хэндлер для удаления сообщения {discriminator}")]
    partial void CouldNotFindHandler(string discriminator);
}
