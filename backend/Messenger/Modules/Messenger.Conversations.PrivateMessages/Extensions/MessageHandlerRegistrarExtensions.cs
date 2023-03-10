using Messenger.Conversations.Common.MessageActions;
using Messenger.Conversations.Common.MessageActions.GetMessageList;
using Messenger.Conversations.Common.Services;
using Messenger.Conversations.PrivateMessages.Features.PrivateGetMessageListAction;
using Messenger.Conversations.PrivateMessages.Features.PrivateSendMessageAction;

namespace Messenger.Conversations.PrivateMessages.Extensions;

public static class MessageHandlerRegistrarExtensions
{
    public static MessageHandlerRegistrar AddPrivateMessageHandlers(this MessageHandlerRegistrar registrar)
    {
        registrar.AddHandler<PrivateSendMessageActionHandler, SendMessageAction, bool>();
        registrar.AddHandler<PrivateGetMessageListActionHandler, GetMessageListAction, GetMessageListActionResponse>();

        return registrar;
    }
}
