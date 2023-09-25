using System.Security.Claims;
using Messenger.RealTime.Common.Model;
using Messenger.RealTime.Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Messenger.RealTime.Hubs;

[Authorize]
public class UpdatesHub : Hub<IRealtimeClient>
{
    private readonly IUpdateConnectionManager _updateConnectionManager;

    public UpdatesHub(IUpdateConnectionManager updateConnectionManager)
    {
        _updateConnectionManager = updateConnectionManager;
    }

    public override Task OnConnectedAsync()
    {
        string name = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        _updateConnectionManager.AddConnection(Guid.Parse(name!), Context.ConnectionId);
        
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        string name = Context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        _updateConnectionManager.RemoveConnection(Guid.Parse(name!), Context.ConnectionId);

        return base.OnDisconnectedAsync(exception);
    }
}
