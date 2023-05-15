using Messenger.Conversations.Common.Extensions;
using Messenger.Conversations.Common.Models;
using Messenger.Conversations.Common.Models.RealtimeUpdates;
using Messenger.Conversations.GroupChats.Extensions;
using Messenger.Conversations.PrivateMessages.Extensions;
using Messenger.Infrastructure.Json;
using Messenger.Infrastructure.Modules;
using Messenger.RealTime.Common.Model;
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
            .Apply();

        services.Configure<PolymorphismJsonOptions>(
            x =>
            {
                x.AddTypeDefinition<IRealtimeUpdate, NewMessageRealtimeUpdate>();
            });
    }
}
