using Messenger.Auth.Models;

namespace Messenger.Auth.Features.Login;

public record LoginCommandResponse(string Token, string RefreshToken)
{
    public static LoginCommandResponse FromAuthenticationResult(AuthenticationResult authenticationResult)
    {
        return new LoginCommandResponse(authenticationResult.Token, authenticationResult.RefreshToken.Token);
    }
};
