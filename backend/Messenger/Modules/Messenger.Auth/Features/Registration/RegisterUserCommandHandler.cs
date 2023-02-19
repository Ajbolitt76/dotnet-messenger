using Microsoft.AspNetCore.Identity;
using Messenger.Auth.Models;
using Messenger.Auth.Services;
using Messenger.Core.Exceptions;
using Messenger.Core.Model.UserAggregate;
using Messenger.Core.Requests.Abstractions;
using Messenger.Infrastructure.Services;
using Messenger.Infrastructure.User;

namespace Messenger.Auth.Features.Registration;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, RegistrationCommandResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IDbContext _dbContext;
    private readonly UserLoginHelperService _userLoginHelperService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterUserCommandHandler(
        UserManager<ApplicationUser> userManager, 
        IDbContext dbContext,
        UserLoginHelperService userLoginHelperService,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _dbContext = dbContext;
        _userLoginHelperService = userLoginHelperService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }
    
    public async Task<RegistrationCommandResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var ticket = _jwtTokenGenerator.ReadPhoneTicketRequest(request.PhoneTicket)
            ?? throw new UnauthorizedException("TICKET_INVALID");
        
        if(ticket.Scope != PhoneTicketScopes.RegisterTicket)
            throw new UnauthorizedException("TICKET_SCOPE_INVALID");
            
        
        if(_userManager.Users.Any(u => u.PhoneNumber == ticket.Phone))
            throw new AlreadyExistsException("User");

        var (identity, domainUser) = await CreateUser(request, ticket.Phone, cancellationToken);

        var tokens = _userLoginHelperService.AuthenticateUser(identity, domainUser);
        
        await _dbContext.SaveEntitiesAsync(cancellationToken);

        return new RegistrationCommandResponse(tokens.Token, tokens.RefreshToken.Token);
    }

    private async Task<(ApplicationUser identity, RepetUser domainUser)> CreateUser(
        RegisterUserCommand request, 
        string phoneNumber,
        CancellationToken cancellationToken)
    {
        var user = new ApplicationUser
        {
            UserName = request.Username,
            PhoneNumber = phoneNumber,
            PhoneNumberConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        var domainUser = new RepetUser()
        {
            UserName = request.Username,
            PhoneNumber = phoneNumber,
            FirstName = request.FirstName,
            LastName = request.LastName,
            IdentityUserId = user.Id
        };

        await _dbContext.RepetUsers.AddAsync(domainUser, cancellationToken);

        if (!result.Succeeded)
            throw new UnauthorizedException(string.Join("\n", result.Errors.Select(x => x.Description)));
        
        return (user, domainUser);
    }
}