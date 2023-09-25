using Messenger.RealTime.Common.Model;
using Messenger.RealTime.Common.Services;
using Messenger.RealTime.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Messenger.RealTime.Services;

public class UserUpdateWorkerFactory : IUserUpdateWorkerFactory
{
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IHubContext<UpdatesHub, IRealtimeClient> _hubContext;
    private readonly ILoggerFactory _loggerFactory;

    public UserUpdateWorkerFactory(
        IHostApplicationLifetime applicationLifetime,
        IHubContext<UpdatesHub, IRealtimeClient> hubContext,
        ILoggerFactory loggerFactory)
    {
        _applicationLifetime = applicationLifetime;
        _hubContext = hubContext;
        _loggerFactory = loggerFactory;
    }

    public IUserUpdateConnectionWorker CreateForUser(ConnectedUser user)
        => new UserUpdateConnectionWorker(
            user,
            _hubContext,
            _applicationLifetime.ApplicationStopping,
            _loggerFactory.CreateLogger<UserUpdateConnectionWorker>());
}
