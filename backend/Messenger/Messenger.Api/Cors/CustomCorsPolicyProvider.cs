using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;

namespace Messenger.Api.Cors;

public class CustomCorsPolicyProvider : ICorsPolicyProvider
{
    private readonly bool _isDevelopment;
    private static readonly Task<CorsPolicy?> NullResult = Task.FromResult<CorsPolicy?>(null);
    private readonly LocalCorsOptions _options;

    public CustomCorsPolicyProvider(IOptions<LocalCorsOptions> options, IHostEnvironment environment)
    {
        _isDevelopment = environment.IsDevelopment();
        _options = options.Value;
    }
    
    public Task<CorsPolicy?> GetPolicyAsync(HttpContext context, string? policyName)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));

        policyName ??= _options.DefaultPolicyName;

        if (_isDevelopment && context.Request.Headers.Origin == "https://web.postman.co")
            policyName = CorsPolicyNames.PostmanSwaggerCorsPolicy;
        
        if (_options.PolicyMap.TryGetValue(policyName, out var result))
            return result.policyTask!;

        return NullResult;
    }
}
