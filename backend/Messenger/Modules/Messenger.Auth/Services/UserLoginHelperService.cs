using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Messenger.Auth.Models;
using Messenger.Core.Exceptions;
using Messenger.Core.Model.UserAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.Infrastructure.Services;
using Messenger.Infrastructure.User;

namespace Messenger.Auth.Services;

public class UserLoginHelperService
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IDbContext _dbContext;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly int _refreshLifetime = 7 * 24 * 60 * 60;
    private readonly int _tokenLifetime = 2 * 60;

    public UserLoginHelperService(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IDbContext dbContext,
        IHttpContextAccessor contextAccessor,
        IJwtTokenGenerator jwtTokenGenerator,
        IDateTimeProvider dateTimeProvider)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _dbContext = dbContext;
        _contextAccessor = contextAccessor;
        _jwtTokenGenerator = jwtTokenGenerator;
        _dateTimeProvider = dateTimeProvider;
    }

    private RefreshToken GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        var ip = _contextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()
            ?? "localhost";

        // TODO: X-Forwarded-For
        var useragent = _contextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString()
            ?? "unknown";

        return new RefreshToken(
            Convert.ToBase64String(randomNumber),
            _dateTimeProvider.NowUtc,
            _dateTimeProvider.NowUtc.AddSeconds(_refreshLifetime),
            ip,
            useragent);
    }

    public AuthenticationResult AuthenticateUser(ApplicationUser user, MessengerUser domainUser)
    {
        var refreshToken = GenerateRefreshToken();
        var token = _jwtTokenGenerator.GenerateUserToken(domainUser,
            _dateTimeProvider.NowUtc.AddSeconds(_tokenLifetime));

        user.AddRefreshToken(refreshToken);

        return new(token, refreshToken);
    }

    public async Task<AuthenticationResult> HandleTicketLogin(string ticket)
    {
        var ticketInfo = _jwtTokenGenerator.ReadPhoneTicketRequest(ticket)
            ?? throw new UnauthorizedException("TICKET_INVALID");

        if(ticketInfo.Scope != PhoneTicketScopes.LoginTicket)
            throw new UnauthorizedException("TICKET_SCOPE_INVALID");
        
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == ticketInfo.Phone)
            ?? throw new NotFoundException<ApplicationUser>();
        
        var domainUser = await _dbContext.MessengerUsers.FirstOrDefaultAsync(x => x.IdentityUserId == user.Id)
            ?? throw new NotFoundException<MessengerUser>();

        return AuthenticateUser(user, domainUser);
    }

    
    public async Task<AuthenticationResult> HandleUsernamePasswordLogin(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username)
            ?? throw new NotFoundException<ApplicationUser>();
        
        var domainUser = await _dbContext.MessengerUsers.FirstOrDefaultAsync(x => x.IdentityUserId == user.Id)
            ?? throw new NotFoundException<MessengerUser>();
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        
        if (!result.Succeeded)
            throw new UnauthorizedException("PASSWORD_INVALID");

        return AuthenticateUser(user, domainUser);
    }
}