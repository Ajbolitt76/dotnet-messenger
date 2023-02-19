using Messenger.Core.Model.UserAggregate;
using Messenger.Core.Requests.Abstractions;

namespace Messenger.Auth.Features.Registration;

// TODO: Валидация на уровне типов
public record RegisterUserCommand(
    string PhoneTicket,
    string Username,
    string Password,
    string FirstName,
    string LastName
) : ICommand<RegistrationCommandResponse>, IRunInTransaction;