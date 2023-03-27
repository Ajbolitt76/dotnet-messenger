using Messenger.Conversations.Common.Abstractions;
using Messenger.Conversations.Common.Features.ReserveConversationNumberCommand;
using Messenger.Conversations.Common.MessageActions;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;

namespace Messenger.Conversations.GroupChats.MessageActions.GroupSendMessage;

public class GroupSendMessageActionHandler : IMessageActionHandler<SendMessageAction, bool>
{
    private readonly IDomainHandler<ReserveConversationNumberCommand, uint> _reserveNumberHandler;
    private readonly IDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserService _userService;
    public static string MessageType => ConversationTypes.GroupChat;

    public GroupSendMessageActionHandler(
        IDbContext dbContext,
        IDomainHandler<ReserveConversationNumberCommand, uint> reserveNumberHandler,
        IUserService userService,
        IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _reserveNumberHandler = reserveNumberHandler;
        _userService = userService;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<bool> Handle(SendMessageAction request, CancellationToken cancellationToken)
    {
        var messagePosition = 
            await _reserveNumberHandler.Handle(new(request.Conversation.Id), cancellationToken);

        //TODO: Валидация отправки
        //1. Член чата
        //2. Без мута/бана
        //3. Есть пермишн

        var messageData = request.MessageData;
        
        var conversationMessage = new ConversationMessage()
        {
            Attachments = messageData.Attachments,
            SentAt = _dateTimeProvider.NowUtc,
            ConversationId = request.Conversation.Id,
            TextContent = messageData.TextContent, 
            SenderId = _userService.GetUserIdOrThrow(),
            Position = messagePosition,
        };

        _dbContext.ConversationMessages.Add(conversationMessage);
        await _dbContext.SaveEntitiesAsync(cancellationToken);

        return true;
    }
}
