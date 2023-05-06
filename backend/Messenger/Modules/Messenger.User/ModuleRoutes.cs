using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Messenger.Infrastructure.Endpoints;
using Messenger.Infrastructure.Extensions;
using Messenger.User.Feature.GetProfileMainData;
using Messenger.User.Feature.GetUserSubscriptionPlan;
using Messenger.User.Feature.SetUserStatus;
using Messenger.User.Feature.UpdateProfileMainData;

namespace Messenger.User;

public class ModuleRoutes : IEndpointRoot
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGroup("/user")
            .WithTags("Пользователь")
            .AddEndpoint<GetProfileMainDataEndpoint>()
            .AddEndpoint<GetUserSubscriptionPlanEndpoint>()
            .AddEndpoint<UpdateProfileMainDataEndpoint>()
            .AddEndpoint<SetUserStatusEndpoint>();
    }
}
