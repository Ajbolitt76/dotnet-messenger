namespace Messenger.Core.Services.PhoneVerification;

public interface IPhoneVerificationService
{
    Task<PhoneVerificationResponse> RequestVerificationCodeAsync(string phoneNumber);
}