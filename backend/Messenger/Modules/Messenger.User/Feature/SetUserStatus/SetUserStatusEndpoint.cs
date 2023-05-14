using FluentValidation;
using MediatR;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Messenger.Infrastructure.Validation.ValidationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Messenger.User.Feature.SetUserStatus;

public class SetUserStatusEndpoint : IEndpoint
{
    public record SetUserStatusDto(string Status);

    public class DtoValidator : AbstractValidator<SetUserStatusDto>
    {
        public DtoValidator()
        {
            RuleFor(s => s.Status)
                .MaximumLength(30);
        }
    }

    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/me/status",
                async (SetUserStatusDto dto, [FromServices] IMediator mediator, [FromServices] IUserService userService)
                    => Results.Ok(
                        await mediator.Send(
                            new SetUserStatusCommand(userService.UserId!.Value, dto.Status))))
            .RequireAuthorization()
            .AddValidation(c => c.AddFor<SetUserStatusDto>())
            .WithName("SetUserStatus");
    }
}
