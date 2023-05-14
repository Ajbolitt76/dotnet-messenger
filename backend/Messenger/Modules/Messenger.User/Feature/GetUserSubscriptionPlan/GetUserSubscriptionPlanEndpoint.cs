using MediatR;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Messenger.User.Feature.GetUserSubscriptionPlan;

public class GetUserSubscriptionPlanEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/me/sub-plan",
            ([FromServices]IMediator mediatr, [FromServices]IUserService userService) 
                => mediatr.Send(new GetUserSubscriptionPlanQuery(userService.UserId!.Value)))
            .WithDescription("Получает информацию о тарифе подписки пользователя")
            .RequireAuthorization();
    }
}
