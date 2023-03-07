using Messenger.Conversations.Common.Abstractions;

namespace Messenger.Conversations.Common.MessageActions.GetMessageList;

public record GetMessageListAction(
    Guid ConversationId,
    int Count = 40,
    Guid? MessagePointer = default) : IMessageAction<GetMessageListActionResponse>;
