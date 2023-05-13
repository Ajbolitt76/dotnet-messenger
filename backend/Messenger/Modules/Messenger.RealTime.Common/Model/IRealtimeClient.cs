namespace Messenger.RealTime.Common.Model;

public interface IRealtimeClient
{
    Task HandleUpdates(List<IRealtimeUpdate> updateData);
}
