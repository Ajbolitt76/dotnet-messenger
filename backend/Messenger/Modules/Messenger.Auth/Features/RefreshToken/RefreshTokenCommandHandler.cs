using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Messenger.Auth.Services;
using Messenger.Core.Exceptions;
using Messenger.Core.Model.UserAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Infrastructure.Services;
using Messenger.Infrastructure.User;

namespace Messenger.Auth.Features.RefreshToken;

public class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IDbContext _context;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly UserLoginHelperService _userLoginHelperService;

    public RefreshTokenCommandHandler(
        UserManager<ApplicationUser> userManager,
        IDbContext context,
        IJwtTokenGenerator jwtTokenGenerator,
        UserLoginHelperService userLoginHelperService)
    {
        _userManager = userManager;
        _context = context;
        _jwtTokenGenerator = jwtTokenGenerator;
        _userLoginHelperService = userLoginHelperService;
    }

    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var validationParamsIgnoringTime = _jwtTokenGenerator.CloneParameters();

        validationParamsIgnoringTime.ValidateLifetime = false;

        var userId =
            Guid.Parse(
                _jwtTokenGenerator.ReadToken(request.Token, validationParamsIgnoringTime)
                    .FindFirst(x => x.Type == ClaimTypes.NameIdentifier)
                    ?.Value
                ?? throw new UnauthorizedException("INVALID_OLD_TOKEN"));

        var user = await _context.RepetUsers.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException<RepetUser>();

        var applicationUser = await _userManager.FindByIdAsync(user.IdentityUserId.ToString())
            ?? throw new NotFoundException<ApplicationUser>();
        
        var token = applicationUser?.RefreshTokens.FirstOrDefault(x => x.Token == request.RefreshToken)
            ?? throw new UnauthorizedException("INVALID_REFRESH_TOKEN");

        applicationUser.RemoveRefreshToken(token);
        
        var result = _userLoginHelperService.AuthenticateUser(applicationUser, user);

        await _context.SaveEntitiesAsync(cancellationToken);
        
        return new(result.Token, result.RefreshToken.Token);
    }
}
