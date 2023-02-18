namespace Messenger.Auth.Models;

public class AuthConfig
{
    public const string ConfigSectionName = "AuthConfig";

    public int UserTokenLifetime { get; set; } = 2 * 60;

    public int PhoneTicketLifetime { get; set; } = 2 * 60;

    public int PhoneTicketRequestLifetime { get; set; } = 5 * 60;

    public int PhoneTicketCooldown { get; set; } = 2 * 60;
}
