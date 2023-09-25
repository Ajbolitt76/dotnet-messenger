using Messenger.Infrastructure.Endpoints;
using Messenger.RealTime.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Messenger.RealTime;

public class RealtimeRoutes : IEndpointRoot
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHub<UpdatesHub>("/realtime");
    }
}
