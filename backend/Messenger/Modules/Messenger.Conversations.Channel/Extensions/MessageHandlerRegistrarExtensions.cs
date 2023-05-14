using Messenger.Conversations.Channel.MessageActions.ChannelGetMessageList;
using Messenger.Conversations.Channel.MessageActions.ChannelSendMessage;
using Messenger.Conversations.Common.MessageActions.GetMessageList;
using Messenger.Conversations.Common.MessageActions.SendMessage;
using Messenger.Conversations.Common.Services;

namespace Messenger.Conversations.Channel.Extensions;

public static class MessageHandlerRegistrarExtensions
{
    public static MessageHandlerRegistrar AddChannelMessageHandlers(this MessageHandlerRegistrar registrar)
    {        
        registrar.AddHandler<ChannelGetMessageListActionHandler, GetMessageListAction, GetMessageListActionResponse>();
        registrar.AddHandler<ChannelSendMessageActionHandler, SendMessageAction, SendMessageActionResponse>();
        //registrar.AddHandler<GroupDeleteMessageActionHandler, DeleteMessageAction, bool>();
        return registrar;
    }
}
