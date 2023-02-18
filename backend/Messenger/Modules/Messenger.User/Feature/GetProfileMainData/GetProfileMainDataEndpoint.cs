using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;

namespace Messenger.User.Feature.GetProfileMainData;

public class GetProfileMainDataEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/me",
            ([FromServices]IMediator mediatr, [FromServices]IUserService userService) =>
            {
                return mediatr.Send(new GetProfileMainDataQuery(
                    userService.UserId!.Value));
            })
            .WithDescription("Получает основную информацию о пользователе")
            .RequireAuthorization();
    }
}
