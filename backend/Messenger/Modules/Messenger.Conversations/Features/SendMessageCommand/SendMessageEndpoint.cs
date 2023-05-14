using FluentValidation;
using MediatR;
using Messenger.Core.Extensions;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Messenger.Infrastructure.Validation.ValidationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.Features.SendMessageCommand;

public class SendMessageEndpoint : IEndpoint
{
    public record SendMessageDto(string Message);

    public class DtoValidator : AbstractValidator<SendMessageDto>
    {
        public DtoValidator()
        {
            RuleFor(x => x.Message)
                .MinimumLength(1)
                .WithLocalizationState()
                .MaximumLength(4000)
                .WithLocalizationState();
        }
    }

    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "/{conversationId:guid}/send",
                async (SendMessageDto dto, Guid conversationId, IMediator mediator, [FromServices] IUserService userService)
                    => Results.Ok(
                        await mediator.Send(
                            new SendMessageCommand(conversationId, userService.UserId!.Value, dto.Message))))
            .RequireAuthorization()
            .AddValidation(c => c.AddFor<SendMessageDto>())
            .WithName("Отправить сообщение в переписку");
    }
}
