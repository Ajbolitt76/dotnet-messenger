using Messenger.Conversations.Common.MessageActions.GetMessageList;
using Messenger.Core.Requests.Abstractions;

namespace Messenger.Conversations.Features.GetMessages;

public record GetMessagesQuery(
    Guid ConversationId,
    int Count = 40,
    Guid? MessagePointer = default) : IQuery<GetMessageListActionResponse>;
