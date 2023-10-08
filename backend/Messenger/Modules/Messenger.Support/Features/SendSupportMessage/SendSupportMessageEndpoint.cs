using FluentValidation;
using MediatR;
using Messenger.Core.Extensions;
using Messenger.Infrastructure.Endpoints;
using Messenger.Infrastructure.Validation.ValidationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Support.Features.SendSupportMessage;

public class SendSupportMessageEndpoint : IEndpoint
{
    public record SendSupportMessageDto(string Message);

    public class DtoValidator : AbstractValidator<SendSupportMessageDto>
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
                async (SendSupportMessageDto dto, Guid conversationId, IMediator mediator)
                    => Results.Ok(
                        await mediator.Send(
                            new SendSupportMessageCommand(conversationId, dto.Message))))
            .RequireAuthorization()
            .AddValidation(c => c.AddFor<SendSupportMessageDto>())
            .WithName("Отправить сообщение в переписку поддержки");
    }
}