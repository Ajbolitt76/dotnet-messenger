using MediatR;
using Messenger.Auth.Services;
using Messenger.Core.Exceptions;
using Messenger.Core.Requests.Abstractions;

namespace Messenger.Auth.Features.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandResponse>
{
    private readonly IDbContext _dbContext;
    private readonly UserLoginHelperService _userLoginHelperService;

    public LoginCommandHandler(IDbContext dbContext, UserLoginHelperService userLoginHelperService)
    {
        _dbContext = dbContext;
        _userLoginHelperService = userLoginHelperService;
    }

    public async Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = LoginCommandResponse.FromAuthenticationResult(
            await (
                request.LoginMode switch
                {
                    LoginMode.Phone => _userLoginHelperService.HandleTicketLogin(
                        request.PhoneTicket
                        ?? throw new ValidationFailedException(nameof(request.PhoneTicket), "PHONE_TICKET_IS_NULL")),

                    LoginMode.Username => _userLoginHelperService.HandleUsernamePasswordLogin(
                        request.Username
                        ?? throw new ValidationFailedException(nameof(request.Username), "USERNAME_IS_NULL"),
                        request.Password
                        ?? throw new ValidationFailedException(nameof(request.Password), "PASSWORD_IS_NULL")),
                }));

        await _dbContext.SaveEntitiesAsync(cancellationToken);
        return result;
    }
}