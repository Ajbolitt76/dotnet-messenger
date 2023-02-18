namespace Messenger.Auth.Features.Registration;

public record RegistrationCommandResponse(string Token, string RefreshToken);