using Messenger.Core.Model.UserAggregate;
using Messenger.Core.Requests.Abstractions;

namespace Messenger.Auth.Features.Registration;

// TODO: Валидация на уровне типов
public record RegisterUserCommand(
    string Name, 
    string PhoneTicket,
    string Username,
    string Password
) : ICommand<RegistrationCommandResponse>, IRunInTransaction;