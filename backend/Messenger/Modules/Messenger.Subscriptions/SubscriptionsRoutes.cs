using Messenger.Infrastructure.Endpoints;
using Messenger.Infrastructure.Extensions;
using Messenger.Subscriptions.Features.PurchaseSubscriptionCommand;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Subscriptions;

public class SubscriptionsRoutes : IEndpointRoot
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGroup("/subscriptions")
            .WithTags("Подписки")
            .AddEndpoint<PurchaseSubscriptionEndpoint>();
    }
}
