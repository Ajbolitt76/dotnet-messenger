using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Messenger.Crypto.Services;

namespace Messenger.Crypto.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddModuleSignatureVerification<TId, TOpts>(
        this IServiceCollection sc,
        Func<TOpts, byte[]?> getPublicKey,
        Func<TOpts, byte[]?> getPrivateKey
    )
        where TOpts : class
    {
        sc.AddScoped<ModuleSignatureService<TId>>(
            (sp) =>
                new ModuleSignatureService<TId>(
                    sp.GetRequiredService<ICryptoService>(),
                    getPublicKey(sp.GetRequiredService<IOptions<TOpts>>().Value),
                    getPrivateKey(sp.GetRequiredService<IOptions<TOpts>>().Value))
        );
        return sc;
    }
}
