using Microsoft.Extensions.Options;
using Messenger.Auth.Models;
using Messenger.Core.Requests.Abstractions;
using Messenger.Core.Services;
using Messenger.Core.Services.PhoneVerification;

namespace Messenger.Auth.Features.PhoneTicket.RequestPhoneTicket;

public class RequestPhoneTicketCommandHandler : IDomainHandler<RequestPhoneTicketCommand, RequestPhoneTicketResponse>
{
    private readonly IPhoneVerificationService _phoneVerificationService;
    private readonly IRedisStore<PersistedPhoneTicketRequest> _redisStore;
    private readonly AuthConfig _authConfig;

    public RequestPhoneTicketCommandHandler(
        IPhoneVerificationService phoneVerificationService,
        IRedisStore<PersistedPhoneTicketRequest> redisStore,
        IOptions<AuthConfig> authConfig)
    {
        _phoneVerificationService = phoneVerificationService;
        _redisStore = redisStore;
        _authConfig = authConfig.Value;
    }

    public async Task<RequestPhoneTicketResponse> Handle(RequestPhoneTicketCommand command,
        CancellationToken cancellationToken)
    {
        var key = command.GetRedisKey();
        var oldTicket = await _redisStore.GetAsync(key);

        if (oldTicket != null && (oldTicket.NextTry - DateTime.UtcNow) > TimeSpan.Zero)
            return new RequestPhoneTicketResponse(oldTicket.NextTry, false);
        
        var result = await _phoneVerificationService.RequestVerificationCodeAsync(command.Phone);

        var expiresAt = TimeSpan.FromSeconds(_authConfig.PhoneTicketRequestLifetime);
        var nextRequestAt = DateTime.UtcNow.AddSeconds(_authConfig.PhoneTicketCooldown);
        
        await _redisStore.SetAsync(key, new PersistedPhoneTicketRequest(command.Phone, result.Code, nextRequestAt, command.Scope), expiresAt);

        return new RequestPhoneTicketResponse(nextRequestAt, true);
    }
}