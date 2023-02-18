using Microsoft.AspNetCore.Routing;

namespace Messenger.Infrastructure.Endpoints;

public interface IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints);
}