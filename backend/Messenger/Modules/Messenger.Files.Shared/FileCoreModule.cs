using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Messenger.Crypto.Extensions;
using Messenger.Files.Shared.Services;
using Messenger.Infrastructure.Modules;

namespace Messenger.Files.Shared;

public class FileCoreModule : IModule
{
    public static void RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IFileUrlService, FileUrlService>();
        services.AddModuleSignatureVerification<FileCoreModule, FileModuleOptions>(
            x => x.LinkSigningKeyPair.PublicKeyBytes!);
    }
}
