using AutoDokas.Services;
using AutoDokas.Services.Options;
using AutoDokas.Services.Options.Factories;
using Microsoft.Extensions.Options;

namespace AutoDokas.Extensions;

/// <summary>
/// Extension methods for configuring AWS SSM-related services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds AWS SSM configuration services to the service collection
    /// </summary>
    public static IServiceCollection AddAwsSsmConfiguration(this IServiceCollection services)
    {
        // Register the SSM configuration service as singleton instead of scoped
        services.AddSingleton<AwsSsmConfigurationService>();

        // Register a factory for AmazonSesOptions that loads from SSM
        services.AddSingleton<IOptionsFactory<AmazonSesOptions>, SesOptionsFactory>();

        // Use the factory to create options
        services.AddSingleton<IOptions<AmazonSesOptions>>(sp =>
            new OptionsWrapper<AmazonSesOptions>(
                sp.GetRequiredService<IOptionsFactory<AmazonSesOptions>>().Create(Options.DefaultName)
            )
        );

        return services;
    }

    /// <summary>
    /// Adds email services to the service collection based on the environment
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="environment">The hosting environment</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddEmailServices(this IServiceCollection services, IHostEnvironment environment)
    {
        if (!environment.IsDevelopment())
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