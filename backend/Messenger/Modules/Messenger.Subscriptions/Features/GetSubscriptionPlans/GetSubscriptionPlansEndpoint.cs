using MediatR;
using Messenger.Infrastructure.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Subscriptions.Features.GetSubscriptionPlans;

public class GetSubscriptionPlansEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/plans-info",
            ([FromServices]IMediator mediatr) => mediatr.Send(new GetSubscriptionPlansQuery()))
            .WithDescription("Получить информацию о планах подписки");
    }
}
