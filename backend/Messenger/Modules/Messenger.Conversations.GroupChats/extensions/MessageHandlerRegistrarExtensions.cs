using Messenger.Conversations.Common.MessageActions.GetMessageList;
using Messenger.Conversations.Common.MessageActions.SendMessage;
using Messenger.Conversations.Common.Services;
using Messenger.Conversations.GroupChats.MessageActions.GroupGetMessageList;
using Messenger.Conversations.GroupChats.MessageActions.GroupSendMessage;

namespace Messenger.Conversations.GroupChats.Extensions;

public static class MessageHandlerRegistrarExtensions
{
    public static MessageHandlerRegistrar AddGroupMessageHandlers(this MessageHandlerRegistrar registrar)
    {        
        registrar.AddHandler<GroupGetMessageListActionHandler, GetMessageListAction, GetMessageListActionResponse>();
        registrar.AddHandler<GroupSendMessageActionHandler, SendMessageAction, SendMessageActionResponse>();
        return registrar;
    }
}
