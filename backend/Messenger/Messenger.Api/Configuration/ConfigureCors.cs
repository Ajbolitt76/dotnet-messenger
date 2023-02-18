namespace Messenger.Api.Configuration;

public static class ConfigureCors
{
    private const string DevCorsPolicyName = "Development";
    private const string ProdCorsPolicyName = "Production";

    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(DevCorsPolicyName,
                builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("Location", "Upload-Offset", "Upload-Length", "SystemFile-Id", "SystemFile-Signed")
                );

            options.AddPolicy(ProdCorsPolicyName,
                builder => builder
                    .WithOrigins("https://repet.online", "https://*.repet.online")
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });
        return services;
    }
    
    public static IApplicationBuilder UseCorsConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseCors(DevCorsPolicyName);
        }
        else
        {
            app.UseCors(ProdCorsPolicyName);
        }
        
        return app;
    }
}
