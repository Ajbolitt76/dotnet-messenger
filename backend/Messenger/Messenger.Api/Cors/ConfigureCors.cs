using Microsoft.AspNetCore.Cors.Infrastructure;

namespace Messenger.Api.Cors;

public static class ConfigureCors
{
    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddTransient<ICorsPolicyProvider, CustomCorsPolicyProvider>();
        services.AddCors();
        services.Configure<LocalCorsOptions>(options =>
        {
            options.AddPolicy(CorsPolicyNames.DevelopmentCorsPolicy,
                builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("Location", "Upload-Offset", "Upload-Length", "SystemFile-Id", "SystemFile-Signed")
                );

            options.AddPolicy(CorsPolicyNames.PostmanSwaggerCorsPolicy,
                builder => builder.WithOrigins("https://web.postman.co")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithExposedHeaders("Location", "Upload-Offset", "Upload-Length", "SystemFile-Id", "SystemFile-Signed")
            );
        });
        return services;
    }
    
    public static IApplicationBuilder UseCorsConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseCors(CorsPolicyNames.DevelopmentCorsPolicy);
        else
            app.UseCors(CorsPolicyNames.DevelopmentCorsPolicy);

        return app;
    }
}
