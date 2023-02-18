using Microsoft.AspNetCore.Routing;

namespace Messenger.Infrastructure.Endpoints;

public interface IEndpointRoot
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints);
}