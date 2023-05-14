using Messenger.Conversations.Channel.Extensions;
using Messenger.Conversations.Common.Abstractions;
using Messenger.Conversations.Common.MessageActions;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.Infrastructure.Extensions;

namespace Messenger.Conversations.Channel.MessageActions.ChannelDeleteMessage;

public class ChannelDeleteMessageActionHandler: IMessageActionHandler<DeleteMessageAction, bool>
{
    private readonly IDbContext _dbContext;
    private readonly IUserService _userService;

    public static string MessageType => ConversationTypes.Channel;
    
    public ChannelDeleteMessageActionHandler(IDbContext dbContext, IUserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    public async Task<bool> Handle(DeleteMessageAction request, CancellationToken cancellationToken)
    {
        var message = await _dbContext.ConversationMessages.FirstOrNotFoundAsync(
            message => message.Id == request.MessageId && message.ConversationId == request.ConversationId,
            cancellationToken: cancellationToken);
        
        (await _dbContext.ChannelMembers.GetChannelMemberOrThrowAsync(
                _userService.GetUserIdOrThrow(),
                request.ConversationId))
            .CheckForPermissionsAndThrow(ChannelMemberPermissions.DeletePosts);
        
        _dbContext.ConversationMessages.Remove(message);
        request.Conversation.HardDeletedCount++;

        await _dbContext.SaveEntitiesAsync(cancellationToken);

        return true;
    }
}
