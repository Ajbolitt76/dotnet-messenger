using Messenger.Conversations.Common.MessageActions;
using Messenger.Conversations.Common.MessageActions.GetMessageList;
using Messenger.Conversations.Common.MessageActions.SendMessage;
using Messenger.Conversations.Common.Services;
using Messenger.Conversations.PrivateMessages.MessageActions.PrivateDeleteMessage;
using Messenger.Conversations.PrivateMessages.MessageActions.PrivateGetMessageList;
using Messenger.Conversations.PrivateMessages.MessageActions.PrivateSendMessage;

namespace Messenger.Conversations.PrivateMessages.Extensions;

public static class MessageHandlerRegistrarExtensions
{
    public static MessageHandlerRegistrar AddPrivateMessageHandlers(this MessageHandlerRegistrar registrar)
    {
        registrar.AddHandler<PrivateSendMessageActionHandler, SendMessageAction, SendMessageActionResponse>();
        registrar.AddHandler<PrivateGetMessageListActionHandler, GetMessageListAction, GetMessageListActionResponse>();
        registrar.AddHandler<PrivateDeleteMessageActionHandler, DeleteMessageAction, bool>();
        return registrar;
    }
}
