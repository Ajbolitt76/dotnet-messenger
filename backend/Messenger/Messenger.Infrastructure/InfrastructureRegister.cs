using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Messenger.Core.Services;
using Messenger.Core.Services.PhoneVerification;
using Messenger.Infrastructure.Services;

namespace Messenger.Infrastructure;

public static class InfrastructureRegister
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(typeof(IRedisStore<>), typeof(RedisStore<>));
        services.AddScoped<IPhoneVerificationService, ConsolePhoneVerificationService>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IMapper, ServiceMapper>();
        return services;
    }
}