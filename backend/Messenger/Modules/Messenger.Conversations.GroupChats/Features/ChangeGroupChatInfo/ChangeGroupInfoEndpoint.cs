using FluentValidation;
using MediatR;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Messenger.Infrastructure.Validation.ValidationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.GroupChats.Features.ChangeGroupChatInfo;

public class ChangeGroupInfoEndpoint : IEndpoint
{
    public record ChangeGroupInfoDto(string NewTitle, string NewDescription); //TODO добавление файла для картинки чата
    
    public class DtoValidator : AbstractValidator<ChangeGroupInfoDto>
    {
        public DtoValidator()
        {
            RuleFor(dto => dto.NewTitle)
                .NotEmpty()
                .MaximumLength(30);
            
            RuleFor(dto => dto.NewDescription)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
    
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "{conversationId:guid}/info",
                async (
                        ChangeGroupInfoDto dto,
                        Guid conversationId,
                        IMediator mediator,
                        IUserService userService)
                    => Results.Ok(
                        await mediator.Send(
                            new ChangeGroupInfoCommand(
                                userService.GetUserIdOrThrow(),
                                conversationId,
                                dto.NewTitle,
                                dto.NewDescription))))
            .RequireAuthorization()
            .Produces<bool>()
            .AddValidation(builder => builder.AddFor<ChangeGroupInfoDto>())
            .WithName("Изменить информацию группового чата");
    }
}
