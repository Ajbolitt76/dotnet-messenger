using System.Threading.Channels;

namespace Messenger.RealTime.Common.Model;

public class ConnectedUser : IDisposable
{
    private readonly HashSet<string> _connections = new();
    
    private Channel<IRealtimeUpdate> _pendingMessages = Channel.CreateBounded<IRealtimeUpdate>(
        new BoundedChannelOptions(100)
        {
            FullMode = BoundedChannelFullMode.DropWrite,
            SingleReader = true,
            SingleWriter = false
        });


    public ConnectedUser(Guid userId)
    {
        UserId = userId.ToString();
    }

    public string UserId { get; }
    
    public DateTime LastUpdate { get; private set; }

    public bool EnlistMessage(IRealtimeUpdate msg) => _pendingMessages.Writer.TryWrite(msg);

    public ChannelReader<IRealtimeUpdate> Messages => _pendingMessages.Reader;

    public int ConnectionCount => _connections.Count;
    
    public void AddConnection(string connectionId)
    {
        lock (_connections)
        {
            _connections.Add(connectionId);
        }
    }

    public IEnumerable<string> Connections => _connections;

    public bool RemoveConnection(string connectionId)
    {
        lock (_connections)
        {
            _connections.Remove(connectionId);
            return _connections.Count != 0;
        }
    }

    public void Dispose()
    {
        _pendingMessages.Writer.Complete();
    }
}
