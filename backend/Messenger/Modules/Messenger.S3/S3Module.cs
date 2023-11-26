using Messenger.Infrastructure.Modules;
using Messenger.S3.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace Messenger.S3;

public class S3Module : IModule
{
    public static void RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<S3Options>()
            .Bind(configuration.GetSection(nameof(S3Options)));

        var config = configuration.GetSection(nameof(S3Options)).Get<S3Options>()
                     ?? throw new ArgumentNullException("Config for s3 is not set");

        services.AddMinio(configureClient => configureClient
            .WithSSL(false)
            .WithEndpoint(config.ServiceUrl)
            .WithCredentials(config.AccessKey, config.SecretKey));

        services.AddScoped<IS3Uploader, S3Uploader>();
    }
}
