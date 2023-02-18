using Microsoft.Extensions.Options;
using Messenger.Auth.Models;
using Messenger.Core.Exceptions;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.Infrastructure.Services;

namespace Messenger.Auth.Features.PhoneTicket.AcquirePhoneTicket;

public class AcquirePhoneTicketCommandHandler : ICommandHandler<AcquirePhoneTicketCommand, AcquirePhoneTicketResponse>
{
    private readonly IRedisStore<PersistedPhoneTicketRequest> _redisStore;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly AuthConfig _authConfig;

    public AcquirePhoneTicketCommandHandler(
        IRedisStore<PersistedPhoneTicketRequest> redisStore,
        IJwtTokenGenerator jwtTokenGenerator,
        IOptions<AuthConfig> authOptions)
    {
        _redisStore = redisStore;
        _jwtTokenGenerator = jwtTokenGenerator;
        _authConfig = authOptions.Value;
    }

    public async Task<AcquirePhoneTicketResponse> Handle(AcquirePhoneTicketCommand request,
        CancellationToken cancellationToken)
    {
        var ticket = await _redisStore.GetAsync(request.GetRedisKey());

        if (ticket is null)
            throw new NotFoundException<AcquirePhoneTicketResponse>();

        if (ticket.Code != request.Code)
            throw new ValidationFailedException("code","INVALID_CODE");
        
        await _redisStore.DeleteAsync(request.GetRedisKey());

        return new AcquirePhoneTicketResponse(
            _jwtTokenGenerator.GenerateToken(new TicketMetadata(ticket.PhoneNumber, ticket.Scope),
                DateTime.UtcNow.AddSeconds(_authConfig.PhoneTicketLifetime)));
    }
}