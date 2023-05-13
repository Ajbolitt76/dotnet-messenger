namespace Messenger.RealTime.Common.Services;

public interface IUserUpdateConnectionWorker : IDisposable
{
    Task Start();
    Task Stop();
}
