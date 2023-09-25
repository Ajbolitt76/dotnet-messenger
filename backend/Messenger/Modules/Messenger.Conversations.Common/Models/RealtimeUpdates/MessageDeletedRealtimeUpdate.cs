using Messenger.RealTime.Common.Model;

namespace Messenger.Conversations.Common.Models.RealtimeUpdates;

public record MessageDeletedRealtimeUpdate(
    Guid ConversationId,
    Guid MessageId) : IRealtimeUpdate
{
    public static string Discriminator => "DELETED_MESSAGE";
}
