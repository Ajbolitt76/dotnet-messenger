using FluentValidation;
using MediatR;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Messenger.Infrastructure.Validation.ValidationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.GroupChats.Features.InviteToGroupChat;

public class InviteToGroupChatEndpoint : IEndpoint
{
    public record InviteToGroupDto(IEnumerable<Guid> InvitedMembers);
    
    public class DtoValidator : AbstractValidator<InviteToGroupDto>
    {
        public DtoValidator()
        {
            RuleFor(x => x.InvitedMembers)
                .NotEmpty();
        }
    }
    
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "{conversationId:guid}/invite",
                async (Guid conversationId, InviteToGroupDto dto, IMediator mediator, IUserService userService)
                    => Results.Ok(
                        await mediator.Send(new InviteToGroupChatCommand(
                            userService.GetUserIdOrThrow(),
                            dto.InvitedMembers ,
                            conversationId))))
            .RequireAuthorization()
            .AddValidation(c => c.AddFor<InviteToGroupDto>())
            .WithName("Пригласить в групповой чат");
    }
}
