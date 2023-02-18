using Messenger.Core.Requests.Abstractions;

namespace Messenger.Auth.Features.Login;

public enum LoginMode
{
    Phone,
    Username
}

// TODO: SmartEnum??
public record LoginCommand(
    LoginMode LoginMode,
    string? PhoneTicket,
    string? Username,
    string? Password
) : ICommand<LoginCommandResponse>;