using FluentValidation;
using MediatR;
using Messenger.Core.Extensions;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Messenger.Infrastructure.Validation.ValidationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.Channel.Features.CreateChannel;

public class CreateChannelEndpoint : IEndpoint
{
    public record CreateChannelDto(string Name);

    public class DtoValidator : AbstractValidator<CreateChannelDto>
    {
        public DtoValidator()
        {
            RuleFor(x => x.Name)
                .MinimumLength(1)
                .WithLocalizationState()
                .MaximumLength(50)
                .WithLocalizationState();
        }
    }

    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/create",
                async (CreateChannelDto dto, IMediator mediator, IUserService userService)
                    => Results.Ok(
                        await mediator.Send(
                            new CreateChannelCommand(userService.GetUserIdOrThrow(), dto.Name))))
            .RequireAuthorization()
            .AddValidation(c => c.AddFor<CreateChannelDto>())
            .WithName("Создать канал");
    }
}
