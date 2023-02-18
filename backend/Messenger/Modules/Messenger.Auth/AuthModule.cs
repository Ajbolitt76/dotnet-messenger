using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Messenger.Auth.Models;
using Messenger.Auth.Services;
using Messenger.Infrastructure.Modules;

namespace Messenger.Auth;

public class AuthModule : IModule
{
    public static void RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthConfig>(
            configuration.GetSection(AuthConfig.ConfigSectionName));
        services.AddScoped<UserLoginHelperService>();
    }
}
