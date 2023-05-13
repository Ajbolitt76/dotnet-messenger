using Messenger.RealTime.Common.Model;

namespace Messenger.Conversations.Common.Models;

public record NewMessageRealtimeUpdate(
    Guid ConversationId,
    BaseMessageListProjection Data) : IRealtimeUpdate
{
    public static string Discriminator => "NEW_MESSAGE";
}
