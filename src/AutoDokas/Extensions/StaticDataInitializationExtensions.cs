using AutoDokas.Services;
using AutoDokas.Data.Models;

namespace AutoDokas.Extensions;

/// <summary>
/// Extension methods for initializing static data on application startup
/// </summary>
public static class StaticDataInitializationExtensions
{
    /// <summary>
    /// Initializes static data in memory during application startup
    /// </summary>
    /// <param name="app">The web application</param>
    /// <returns>The web application for chaining</returns>
    public static WebApplication InitializeStaticData(this WebApplication app)
    {
        // Create a scope to resolve the static data cache service
        using var scope = app.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<ICsvReader>();

        if (service is not CachedCsvReader staticDataCacheService)
        {
            throw new InvalidOperationException("Static data cache service is not available.");
        }

        // Initialize the static data asynchronously - we use Wait() since this is startup code
        staticDataCacheService.InitializeAsync<Country>("countries.csv").Wait();
        
        return app;
    }
}