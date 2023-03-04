using FluentValidation;
using MediatR;
using Messenger.Core.Extensions;
using Messenger.Core.Requests.Responses;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Messenger.Infrastructure.Validation.ValidationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.GroupChats.Features.CreateGroupChat;

public class CreateGroupChatEndpoint : IEndpoint
{
    public record CreateGroupChatDto(
        string Name,
        IEnumerable<Guid> InvitedMembers);

    public class DtoValidator : AbstractValidator<CreateGroupChatDto>
    {
        public DtoValidator()
        {
            RuleFor(x => x.InvitedMembers)
                .NotEmpty();

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
                async (CreateGroupChatDto dto, IMediator mediator, IUserService userService)
                    => Results.Ok(
                        await mediator.Send(
                            new CreateGroupChatCommand(userService.GetUserIdOrThrow(), dto.InvitedMembers, dto.Name))))
            .RequireAuthorization()
            .AddValidation(c => c.AddFor<CreateGroupChatDto>())
            .Produces<CreatedResponse<Guid>>()
            .WithName("Создать групповой чат");
    }
}
