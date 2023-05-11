using Messenger.Conversations.Channel.Extensions;
using Messenger.Conversations.Common.Extensions;
using Messenger.Conversations.GroupChats.Extensions;
using Messenger.Conversations.PrivateMessages.Extensions;
using Messenger.Infrastructure.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messenger.Conversations;

public class ConversationsModule : IModule
{
    public static void RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddMessageHandlers()
            .AddPrivateMessageHandlers()
            .AddGroupMessageHandlers()
            .AddChannelMessageHandlers()
            .Apply();
    }
}
