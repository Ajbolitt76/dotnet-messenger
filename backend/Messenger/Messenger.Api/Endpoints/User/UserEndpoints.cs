using Messenger.Infrastructure.Endpoints;

namespace Messenger.Api.Endpoints.User;

public class UserEndpoints : IEndpointRoot
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/user")
            .WithTags("User")
            .WithDescription("Эндпоинты для работы с пользователями")
            .WithGroupName("User");
    }
}