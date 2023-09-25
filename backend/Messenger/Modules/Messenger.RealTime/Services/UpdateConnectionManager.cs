using System.Collections.Concurrent;
using Messenger.RealTime.Common.Model;
using Messenger.RealTime.Common.Services;

namespace Messenger.RealTime.Services;

public class UpdateConnectionManager : IUpdateConnectionManager
{
    private readonly IUserUpdateWorkerFactory _userUpdateWorkerFactory;
    private readonly ConcurrentDictionary<Guid, ConnectedUser> _connectedUsers = new();
    private readonly ConcurrentDictionary<Guid, IUserUpdateConnectionWorker> _workers = new();
    public IReadOnlyDictionary<Guid, ConnectedUser> ConnectedUser => _connectedUsers;

    public UpdateConnectionManager(IUserUpdateWorkerFactory userUpdateWorkerFactory)
    {
        _userUpdateWorkerFactory = userUpdateWorkerFactory;
    }

    public void SendToUsers(Guid[] userIds, IRealtimeUpdate update)
    {
        foreach (var userId in userIds)
        {
            if (!_connectedUsers.TryGetValue(userId, out var connectedUser))
                continue;
            connectedUser.EnlistMessage(update);
        }
    }

    public void SendToUser(Guid userId, IRealtimeUpdate update)
    {
        if (!_connectedUsers.TryGetValue(userId, out var connectedUser))
            return;
        connectedUser.EnlistMessage(update);
    }

    public void AddConnection(Guid userId, string connectionId)
    {
        if (!_connectedUsers.TryGetValue(userId, out var cu))
        {
            cu = new ConnectedUser(userId);
            var worker = _userUpdateWorkerFactory.CreateForUser(cu);
            _workers.AddOrUpdate(
                userId, 
                x => worker,
                (_, old) =>
                {
                    old.Stop();
                    return worker;
                });
            _connectedUsers.AddOrUpdate(
                userId,
                _ => cu,
                (_, old) =>
                {
                    old.Dispose();
                    return cu;
                });
            worker.Start();
        }

        cu.AddConnection(connectionId);
    }

    public void RemoveConnection(Guid userId, string connectionId)
    {
        if (_connectedUsers.TryGetValue(userId, out var cu))
        {
            cu.RemoveConnection(connectionId);
            if (cu.ConnectionCount == 0)
            {
                _connectedUsers.Remove(userId, out _);
                if (_workers.Remove(userId, out var worker)) ;
                worker!.Stop();
                cu.Dispose();
            }
        }
    }
}
