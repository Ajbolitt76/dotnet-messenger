﻿using FluentValidation;
using MediatR;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Messenger.Infrastructure.Validation.ValidationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Conversations.GroupChats.Features.MuteGroupMember;

public class MuteGroupMemberEndpoint : IEndpoint
{
    public record MuteDto(int Seconds);
    
    public class DtoValidator : AbstractValidator<MuteDto>
    {
        public DtoValidator()
        {
            RuleFor(x => x.Seconds)
                .GreaterThan(0);
        }
    }
    
    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "{conversationId:guid}/mute/{toUserId:guid}",
                async (
                        MuteDto dto,
                        Guid toUserId,
                        Guid conversationId,
                        IMediator mediator,
                        IUserService userService)
                    => Results.Ok(
                        await mediator.Send(
                            new MuteGroupMemberCommand(
                                userService.GetUserIdOrThrow(),
                                toUserId,
                                conversationId,
                                new TimeSpan(dto.Seconds * 1000 * 1000)))))
            .RequireAuthorization()
            .Produces<bool>()
            .AddValidation(builder => builder.AddFor<MuteDto>())
            .WithName("Замутить участника");
    }
}