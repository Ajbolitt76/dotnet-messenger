using Messenger.Conversations.Common.Abstractions;
using Messenger.Conversations.Common.Features.ReserveConversationNumberCommand;
using Messenger.Conversations.Common.MessageActions;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.Infrastructure.Extensions;

namespace Messenger.Conversations.PrivateMessages.Features.PrivateDeleteMessageAction;

public class PrivateDeleteMessageActionHandler : IMessageActionHandler<DeleteMessageAction, bool> 
{
    private readonly IDbContext _dbContext;
    private readonly IDomainHandler<ReserveConversationNumberCommand, uint> _reserveNumberHandler;
    private readonly IUserService _userService;
    
    public PrivateDeleteMessageActionHandler(
        IDbContext dbContext,
        IDomainHandler<ReserveConversationNumberCommand, uint> reserveNumberHandler,
        IUserService userService)
    {
        _dbContext = dbContext;
        _reserveNumberHandler = reserveNumberHandler;
        _userService = userService;
    }
    
    public static string MessageType => ConversationTypes.PersonalChat;

    public async Task<bool> Handle(DeleteMessageAction request, CancellationToken cancellationToken)
    {
        var message = await _dbContext.ConversationMessages.FirstOrNotFoundAsync(
            x => x.Id == request.MessageId && x.ConversationId == request.ConversationId);
        var clientId = _userService.GetUserIdOrThrow();     
        
        if (!request.DeleteFromAll || clientId != message.SenderId)
        {
            message.DeletedFrom ??= new List<Guid>();
            if (!message.DeletedFrom.Contains(clientId))
                message.DeletedFrom.Add(clientId);
        }
        else
        {
            _dbContext.ConversationMessages.Remove(message);
            request.Conversation.DeletedCount++;
        }
        await _dbContext.SaveEntitiesAsync(cancellationToken);
        
        return true;
    }
}
