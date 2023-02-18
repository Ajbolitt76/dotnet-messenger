using System.Collections.Immutable;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSec.Cryptography;
using Messenger.Crypto.Services;
using Messenger.Infrastructure.Modules;

namespace Messenger.Crypto;

public class CryptoModule : IModule
{
    public static void RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICryptoService, CryptoService>();
    }
}
