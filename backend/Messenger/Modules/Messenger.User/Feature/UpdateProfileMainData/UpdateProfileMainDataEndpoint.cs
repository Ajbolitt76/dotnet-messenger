using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Messenger.Core.Extensions;
using Messenger.Core.Model.UserAggregate;
using Messenger.Core.Services;
using Messenger.Crypto.Models;
using Messenger.Crypto.Services;
using Messenger.Files.Shared;
using Messenger.Files.Shared.FileRequests;
using Messenger.Infrastructure.Endpoints;
using Messenger.Infrastructure.Validation.ValidationFilter;
using FileInfo = Messenger.Files.Shared.FileRequests.FileInfo;

namespace Messenger.User.Feature.UpdateProfileMainData;

public class UpdateProfileMainDataEndpoint : IEndpoint
{
    public record EditProfileDto(
        string Name,
        DateTime? DateOfBirth,
        SignedData<FileInfo>? ProfilePicture);

    public class DtoValidator : AbstractValidator<EditProfileDto>
    {
        public DtoValidator(ModuleSignatureValidator<FileCoreModule> signatureValidator)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithLocalizationState()
                .MaximumLength(50)
                .WithLocalizationState();

            When(
                x => x.DateOfBirth.HasValue,
                () =>
                {
                    RuleFor(x => x.DateOfBirth)
                        .LessThan(x => DateTime.Now.AddYears(-16))
                        .WithLocalizationState()
                        .GreaterThan(x => DateTime.Now.AddYears(-100))
                        .WithLocalizationState();
                }
            );


            When(
                x => x.ProfilePicture != null,
                () =>
                {
                    RuleFor(x => x.ProfilePicture)
                        .Must(signatureValidator.Validate)
                        .WithErrorCode("SignatureInvalid")
                        .WithLocalizationState();
                });
        }
    }

    public void Map(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/me",
                async (EditProfileDto command, IUserService userService, IMediator mediator)
                    => Results.Ok(
                        await mediator.Send(
                            new UpdateProfileMainDataCommand(
                                userService.UserId!.Value,
                                command.Name,
                                command.DateOfBirth,
                                command.ProfilePicture?.Data.FileId
                            ))))
            .RequireAuthorization()
            .AddValidation(c => c.AddFor<EditProfileDto>())
            .WithName("Обновить профиль");
    }
}
