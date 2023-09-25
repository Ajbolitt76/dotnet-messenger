using Messenger.Conversations.Channel.Extensions;
using Messenger.Conversations.Common.Abstractions;
using Messenger.Conversations.Common.Features.ReserveConversationNumberCommand;
using Messenger.Conversations.Common.MessageActions.SendMessage;
using Messenger.Core.Model.ConversationAggregate;
using Messenger.Core.Model.ConversationAggregate.Permissions;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;

namespace Messenger.Conversations.Channel.MessageActions.ChannelSendMessage;

public class ChannelSendMessageActionHandler : IMessageActionHandler<SendMessageAction, SendMessageActionResponse>
{
    private readonly IDomainHandler<ReserveConversationNumberCommand, uint> _reserveNumberHandler;
    private readonly IDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUserService _userService;

    public static string MessageType => ConversationTypes.Channel;
    
    public ChannelSendMessageActionHandler(
        IDomainHandler<ReserveConversationNumberCommand, uint> reserveNumberHandler,
        IDbContext dbContext,
        IDateTimeProvider dateTimeProvider,
        IUserService userService)
    {
        _reserveNumberHandler = reserveNumberHandler;
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
        _userService = userService;
    }

    public async Task<SendMessageActionResponse> Handle(SendMessageAction request, CancellationToken cancellationToken)
    {
        var messagePosition =
            await _reserveNumberHandler.Handle(new(request.Conversation.Id), cancellationToken);
        
        var currentUserId = _userService.GetUserIdOrThrow();

        (await _dbContext.ChannelMembers.GetChannelMemberOrThrowAsync(currentUserId, request.Conversation.Id))
            .CheckForPermissionsAndThrow(ChannelMemberPermissions.AddPosts);
        
        var messageData = request.MessageData;
        
        var conversationMessage = new ConversationMessage
        {
            Attachments = messageData.Attachments,
            SentAt = _dateTimeProvider.NowUtc,
            ConversationId = request.Conversation.Id,
            TextContent = messageData.TextContent,
            SenderId = currentUserId,
            Position = messagePosition,
        };

        _dbContext.ConversationMessages.Add(conversationMessage);
        await _dbContext.SaveEntitiesAsync(cancellationToken);

        return new(true, conversationMessage.Id);
    }
}
