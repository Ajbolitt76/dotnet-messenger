using FluentValidation;
using MediatR;
using Messenger.Core.Extensions;
using Messenger.Infrastructure.Endpoints;
using Messenger.Infrastructure.Validation.ValidationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.Features.GetMessages;

public class GetMessagesEndpoint : IEndpoint
{
    public record GetMessagesRequestDto(
        int Count = 40,
        Guid? MessagePointer = default);

    public class DtoValidator : AbstractValidator<GetMessagesRequestDto>
    {
        public DtoValidator()
        {
            RuleFor(x => x.Count)
                .GreaterThan(0)
                .LessThan(100)
                .WithLocalizationState();
        }
    }
    
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet(
                "/{conversationId:guid}/messages",
                (Guid conversationId, [AsParameters] GetMessagesRequestDto request, [FromServices] IMediator mediatr) =>
                {
                    return mediatr.Send(new GetMessagesQuery(conversationId, request.Count, request.MessagePointer));
                })
            .WithDescription("Получает список сообщений")
            .AddValidation(c => c.AddFor<GetMessagesRequestDto>())
            .RequireAuthorization();
    }
}
