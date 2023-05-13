using Messenger.RealTime.Common.Model;
using Messenger.RealTime.Common.Services;
using Messenger.RealTime.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Messenger.RealTime.Services;

public partial class UserUpdateConnectionWorker : IUserUpdateConnectionWorker
{
    private readonly ConnectedUser _user;
    private readonly IHubContext<UpdatesHub, IRealtimeClient> _hubContext;
    private readonly ILogger<UserUpdateConnectionWorker> _logger;
    private readonly CancellationTokenSource _cts;
    private readonly Random _random = new();
    private Task? _task;
    
    public UserUpdateConnectionWorker(
        ConnectedUser user, 
        IHubContext<UpdatesHub, IRealtimeClient> hubContext,
        CancellationToken globalToken,
        ILogger<UserUpdateConnectionWorker> logger)
    {
        _user = user;
        _hubContext = hubContext;
        _logger = logger;
        _cts = CancellationTokenSource.CreateLinkedTokenSource(globalToken);
    }

    public Task Start()
    {
        _task = DoWork(_cts.Token);
        LogStarted(_user.UserId);
        return _task;
    }

    public Task Stop()
    {
        LogStopping(_user.UserId);
        _cts.Cancel();
        return _task;
    }

    private async Task DoWork(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested 
               && await _user.Messages.WaitToReadAsync(cancellationToken))
        {
            await Task.Delay(_random.Next(100, 1000), cancellationToken);
            var ls = new List<IRealtimeUpdate>(_user.Messages.Count);
            while (_user.Messages.TryRead(out var msg))
            {
                ls.Add(msg);
            }
            await _hubContext.Clients.Clients(_user.Connections).HandleUpdates(ls);
        }
        LogStopped(_user.UserId);
    }

    public void Dispose()
    {
        _cts.Cancel();
    }

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Started update worker for {userId}")]
    partial void LogStarted(string userId);
    
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Stopping update worker for {userId}")]
    partial void LogStopping(string userId);
    
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Stopped update worker for {userId}")]
    partial void LogStopped(string userId);
}
