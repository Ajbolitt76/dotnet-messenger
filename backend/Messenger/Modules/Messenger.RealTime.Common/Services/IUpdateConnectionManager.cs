using Messenger.RealTime.Common.Model;

namespace Messenger.RealTime.Common.Services;

public interface IUpdateConnectionManager
{
    IReadOnlyDictionary<Guid, ConnectedUser> ConnectedUser { get; }

    void AddConnection(Guid userId, string connectionId);

    void RemoveConnection(Guid userId, string connectionId);

    void SendToUsers(Guid[] userIds, IRealtimeUpdate update);

    void SendToUser(Guid userId, IRealtimeUpdate update);
}
