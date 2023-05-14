using MediatR;
using Messenger.Api.Behaviours;
using Messenger.Core.Services;
using Messenger.Infrastructure.Services;
using Messenger.User;

namespace Messenger.Api.Configuration;

public static class ConfigureCoreServices
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddMediatR(typeof(IPipelineBehavior<,>).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MessageValidationBehaviour<,>));
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}