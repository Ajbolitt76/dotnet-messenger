using Microsoft.Extensions.Logging;
using Messenger.Core.Services.PhoneVerification;

namespace Messenger.Infrastructure.Services;

public class ConsolePhoneVerificationService : IPhoneVerificationService
{
    private readonly ILogger<ConsolePhoneVerificationService> _logger;

    public ConsolePhoneVerificationService(ILogger<ConsolePhoneVerificationService> logger)
    {
        _logger = logger;
    }
    
    public Task<PhoneVerificationResponse> RequestVerificationCodeAsync(string phoneNumber)
    {
        var code = new Random().Next(1000, 9999);
        _logger.LogInformation("Verification code {Code} for phone number {PhoneNumber}", code, phoneNumber);
        return Task.FromResult(new PhoneVerificationResponse(code.ToString()));
    }
}