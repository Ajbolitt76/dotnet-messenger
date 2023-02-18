using Serilog;

namespace Messenger.Api.Configuration;

public static class ConfigureLogging
{
    public static void AddCustomLogging(this IHostBuilder hostBuilder)
    {
        hostBuilder.UseSerilog((context, services, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
            configuration.Enrich.FromLogContext();
            configuration.Enrich.WithProperty("Application", "Messenger");
            configuration.Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName);
            configuration.WriteTo.Console();
        });
    }
}