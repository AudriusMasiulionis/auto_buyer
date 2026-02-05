using AutoDokas.Services;

namespace AutoDokas.Extensions;

/// <summary>
/// Extension methods for configuring AWS SSM-related services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds email services to the service collection based on the environment
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="environment">The hosting environment</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddEmailServices(this IServiceCollection services, IHostEnvironment environment)
    {
        services.AddScoped<RazorEmailTemplateFactory>();
        services.AddScoped<EmailNotificationService>();

        if (environment.IsDevelopment())
        {
            // Use Amazon SES in production environment
            services.AddScoped<IEmailService, AmazonSesEmailService>();
    }
        else
        {
            services.AddScoped<IEmailService, FakeEmailService>();
        }

        return services;
    }

    /// <summary>
    /// Adds data services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        services.AddKeyedScoped<ICsvReader, CsvReader>(typeof(CsvReader).Name);
        services.AddScoped<ICsvReader, CachedCsvReader>();
        return services;
    }
}