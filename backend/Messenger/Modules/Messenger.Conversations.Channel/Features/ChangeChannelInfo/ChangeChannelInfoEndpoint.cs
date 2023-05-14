using FluentValidation;
using MediatR;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Messenger.Infrastructure.Validation.ValidationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.Channel.Features.ChangeChannelInfo;

public class ChangeChannelInfoEndpoint : IEndpoint
{
    public record ChangeChannelInfoDto(string NewTitle, string NewDescription); //TODO добавление файла для картинки чата
    
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "{conversationId:guid}/info",
                async (
                        ChangeChannelInfoDto dto,
                        Guid conversationId,
                        IMediator mediator,
                        IUserService userService)
                    => Results.Ok(
                        await mediator.Send(
                            new ChangeChannelInfoCommand(
                                userService.GetUserIdOrThrow(),
                                conversationId,
                                dto.NewTitle,
                                dto.NewDescription))))
            .RequireAuthorization()
            .Produces<bool>()
            .AddValidation(builder => builder.AddFor<ChangeChannelInfoDto>())
            .WithName("Изменить информацию канала");
    }
    
    public class DtoValidator : AbstractValidator<ChangeChannelInfoDto>
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
}
