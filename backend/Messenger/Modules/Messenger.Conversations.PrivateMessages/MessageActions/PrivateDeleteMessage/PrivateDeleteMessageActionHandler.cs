using Messenger.Conversations.Common.Abstractions;
using Messenger.Conversations.Common.Features.ReserveConversationNumberCommand;
using Messenger.Conversations.Common.MessageActions;
using Messenger.Conversations.Common.Models.RealtimeUpdates;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.Infrastructure.Extensions;
using Messenger.RealTime.Common.Services;

namespace Messenger.Conversations.PrivateMessages.MessageActions.PrivateDeleteMessage;

public class PrivateDeleteMessageActionHandler : IMessageActionHandler<DeleteMessageAction, bool>
{
    private readonly IDbContext _dbContext;
    private readonly IDomainHandler<ReserveConversationNumberCommand, uint> _reserveNumberHandler;
    private readonly IUpdateConnectionManager _updateConnectionManager;
    private readonly IUserService _userService;

    public PrivateDeleteMessageActionHandler(
        IDbContext dbContext,
        IDomainHandler<ReserveConversationNumberCommand, uint> reserveNumberHandler,
        IUpdateConnectionManager _updateConnectionManager,
        IUserService userService)
    {
        _dbContext = dbContext;
        _reserveNumberHandler = reserveNumberHandler;
        this._updateConnectionManager = _updateConnectionManager;
        _userService = userService;
    }

    public static string MessageType => ConversationTypes.PersonalChat;

    public async Task<bool> Handle(DeleteMessageAction request, CancellationToken cancellationToken)
    {
        var message = await _dbContext.ConversationMessages.FirstOrNotFoundAsync(
            x => x.Id == request.MessageId && x.ConversationId == request.ConversationId,
            cancellationToken: cancellationToken);
        var clientId = _userService.GetUserIdOrThrow();

        var update = new MessageDeletedRealtimeUpdate(message.ConversationId!.Value, message.Id);

        if (!request.DeleteFromAll || clientId != message.SenderId)
        {
            var conversationStatus = await _dbContext.ConversationUserStatuses.Where(
                x => x.ConversationId == request.MessageId && x.ConversationId == request.ConversationId)
                .FirstOrNotFoundAsync(cancellationToken: cancellationToken);

            conversationStatus.SoftDeletedCount++;
            
            message.DeletedFrom ??= new List<Guid>();
            if (!message.DeletedFrom.Contains(clientId))
                message.DeletedFrom.Add(clientId);
            
            _updateConnectionManager.SendToUsers(message.DeletedFrom.ToArray(), update);
        }
        else
        {
            _dbContext.ConversationMessages.Remove(message);

            //TODO: Уменьшать SoftDeletedCount, у пользователя удалившего сооьбщение
            request.Conversation.HardDeletedCount++;
            
            var toNotify = _dbContext.PersonalChatMembers
                .Where(x => x.ConversationId == request.Conversation.Id)
                .Select(x => x.UserId)
                .ToArray();
            _updateConnectionManager.SendToUsers(toNotify, update);
        }

        await _dbContext.SaveEntitiesAsync(cancellationToken);

        return true;
    }
}
