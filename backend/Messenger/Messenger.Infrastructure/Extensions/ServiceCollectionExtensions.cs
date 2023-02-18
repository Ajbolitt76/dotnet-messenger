using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Messenger.Infrastructure.Json;

namespace Messenger.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureWithServices<TOptions>(
        this IServiceCollection sc,
        Action<TOptions, IServiceProvider> configure)
        where TOptions : class
    {
        sc.AddOptions();
        sc.AddSingleton<IConfigureOptions<TOptions>>(
            sp => new ConfigureNamedOptions<TOptions>(
                Options.DefaultName,
                (opts) => configure(opts, sp)));
        return sc;
    }
}
