using Messenger.RealTime.Common.Model;

namespace Messenger.RealTime.Common.Services;

public interface IUserUpdateWorkerFactory
{
    IUserUpdateConnectionWorker CreateForUser(ConnectedUser user);
}
