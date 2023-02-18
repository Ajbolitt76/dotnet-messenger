namespace Messenger.Core.Services.Email;

public interface IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string message);
}