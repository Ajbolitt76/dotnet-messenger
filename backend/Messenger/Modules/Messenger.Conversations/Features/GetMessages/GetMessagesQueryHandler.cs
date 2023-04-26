using Messenger.Conversations.Common.MessageActions;
using Messenger.Conversations.Common.MessageActions.GetMessageList;
using Messenger.Conversations.Common.MessageActions.SendMessage;
using Messenger.Conversations.Common.Models;
using Messenger.Conversations.Common.Services;
using Messenger.Core.Exceptions;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

namespace Messenger.Conversations.Features.GetMessages;

public partial class GetMessagesQueryHandler : IQueryHandler<GetMessagesQuery, GetMessageListActionResponse>
{
    private readonly MessageHandlerProvider _messageHandlerProvider;
    private readonly IDbContext _dbContext;
    private readonly ILogger<GetMessagesQueryHandler> _logger;

    public GetMessagesQueryHandler(
        MessageHandlerProvider messageHandlerProvider,
        IDbContext dbContext,
        ILogger<GetMessagesQueryHandler> logger)
    {
        _messageHandlerProvider = messageHandlerProvider;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<GetMessageListActionResponse> Handle(
        GetMessagesQuery request,
        CancellationToken cancellationToken)
    {
        var conversation = await _dbContext.Conversations.FirstOrNotFoundAsync(
            x => x.Id == request.ConversationId,
            cancellationToken: cancellationToken);

        var handler =
            _messageHandlerProvider.GetMessageHandler<GetMessageListAction, GetMessageListActionResponse>(
                conversation.ConversationType);

        if (handler is null)
        {
            CouldNotFindHandler(conversation.ConversationType);
            throw new NotFoundException<SendMessageAction>();
        }

        return await handler.Handle(
            new GetMessageListAction(conversation.Id, request.Count, request.MessagePointer),
            cancellationToken);
    }

    [LoggerMessage(
        EventId = 120,
        Level = LogLevel.Error,
        EventName = "GetMessagesUnknownHandler",
        Message = "Не удалось найти хэндлер для получения списка сообщений {discriminator}")]
    partial void CouldNotFindHandler(string discriminator);
}
