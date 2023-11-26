using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Messenger.Core.Model.FileAggregate.FileLocation;
using Messenger.Crypto.Extensions;
using Messenger.Files.Services;
using Messenger.Files.Shared.FileRequests;
using Messenger.Files.Shared.Services;
using Messenger.Infrastructure.Extensions;
using Messenger.Infrastructure.Modules;
using Messenger.Infrastructure.Validation;

namespace Messenger.Files;

public class FileModule : IModule
{
    public static void RegisterModule(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IValidator<FileModuleOptions>, FileModuleOptions.FileModuleOptionsValidator>();
        
        services.AddOptions<FileModuleOptions>()
            .Bind(configuration.GetSection(nameof(FileModule)))
            .ValidateWithFluentValidation()
            .ValidateOnStart();

        services.AddHostedService<FilePusherHostedService>();
        
        services.AddScoped<TusUploadManager>();
        services.AddScoped<FileServer>();
        services.AddScoped<IFileLocationServer<TusFileLocation>, TusFileServer>();
        services.AddScoped<IFileLocationServer<S3FileLocation>, S3FileServer>();
    }
}
