using FluentValidation;
using MediatR;
using Messenger.Core.Services;
using Messenger.Infrastructure.Endpoints;
using Messenger.Infrastructure.Validation.ValidationFilter;
using Messenger.SubscriptionPlans.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Messenger.Subscriptions.Features.PurchaseSubscriptionCommand;

public class PurchaseSubscriptionEndpoint : IEndpoint
{
    public record PurchaseSubscriptionDto(
        string CardNumber,
        string ExpirationDate,
        string HolderName,
        string Cvv,
        uint Plan,
        uint Months);

    public class DtoValidator : AbstractValidator<PurchaseSubscriptionDto>
    {
        public DtoValidator()
        {
            RuleFor(pi => pi.CardNumber)
                .NotEmpty()
                .Matches(@"^\d+$")
                .MinimumLength(13)
                .MaximumLength(16);
            RuleFor(cardData => cardData.ExpirationDate)
                .NotEmpty()
                .Matches(@"^(0[1-9]|1[0-2])\/([0-9]{2})$")
                .WithMessage("Expiration Date should be in the format MM/YY.");
            RuleFor(pi => pi.HolderName)
                .NotEmpty()
                .Matches(@"^[a-zA-Z]+(\s[a-zA-Z]+)*$")
                .MinimumLength(5)
                .MaximumLength(50);
            RuleFor(pi => pi.Cvv)
                .NotEmpty()
                .Matches(@"^\d+$")
                .MinimumLength(3)
                .MaximumLength(3);
            RuleFor(pi => pi.Plan)
                .NotEmpty()
                .Must(t => Enum.IsDefined(typeof(Plan), t))
                .WithMessage("INCORRECT_PLAN");
            RuleFor(pi => pi.Months)
                .NotEmpty()
                .Must(m => m > 0)
                .WithMessage("MIN_ONE_MONTH");
        }
    }

    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost(
                "purchase",
                async (PurchaseSubscriptionDto dto, [FromServices]IMediator mediator, [FromServices]IUserService userService)
                    => Results.Ok(
                        await mediator.Send(
                            new PurchaseSubscriptionCommand(userService.UserId!.Value, dto.CardNumber, dto.ExpirationDate, dto.HolderName, dto.Cvv, dto.Plan, dto.Months))))
            .RequireAuthorization()
            .AddValidation(c => c.AddFor<PurchaseSubscriptionDto>())
            .WithName("Получить подписку");
    }
}
