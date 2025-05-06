using System.Globalization;
using Microsoft.Extensions.Caching.Memory;

namespace AutoDokas.Services;

/// <summary>
/// Implementation of the IStaticDataCacheService interface for caching static data in memory
/// </summary>
public class CachedCsvReader : ICsvReader
{
    private readonly ICsvReader _csvReader;
    private readonly IMemoryCache _cache;
    private readonly ILogger<CachedCsvReader> _logger;

    /// <summary>
    /// Initializes a new instance of the StaticDataCacheService class
    /// </summary>
    /// <param name="csvReader">The CSV data service</param>
    /// <param name="logger">The logger</param>
    public CachedCsvReader(
        [FromKeyedServices(nameof(CsvReader))] ICsvReader csvReader,
        IMemoryCache cache,
        ILogger<CachedCsvReader> logger)
    {
        _csvReader = csvReader ?? throw new ArgumentNullException(nameof(csvReader));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<T>> ReadAllAsync<T>(string filePath)
    {
        var key = typeof(T).Name;

        if (_cache.TryGetValue(key, out var data) && data is IEnumerable<T> typedData)
        {
            return typedData;
        }
        else
        {
            typedData = await _csvReader.ReadAllAsync<T>(filePath);
            _cache.Set(key, data, TimeSpan.FromHours(24 * 5));
            return typedData;
        }
    }

    public Task<IEnumerable<T>> ReadAllAsync<T>(string filePath, CultureInfo culture)
    {
        var key = typeof(T).Name;

        if (_cache.TryGetValue(key, out var data) && data is IEnumerable<T> typedData)
        {
            return Task.FromResult(typedData);
        }
        else
        {
            typedData = _csvReader.ReadAllAsync<T>(filePath, culture).Result;
            _cache.Set(key, data, TimeSpan.FromHours(24 * 5));
            return Task.FromResult(typedData);
        }
    }


    /// <summary>
    /// Initializes the static data cache by reading data from a CSV file and storing it in memory
    /// </summary>
    /// <typeparam name="T">The data type</typeparam>
    /// <param name="dataPath">Name or path to the static file</param>
    public async Task InitializeAsync<T>(string dataPath)
    {
        try
        {
            var key = typeof(T).Name;
            var data = await _csvReader.ReadAllAsync<T>(dataPath);

            _logger.LogInformation("Read {Count} items from {Path}", data.Count(), dataPath);
            _cache.Set(key, data, TimeSpan.FromHours(24 * 5));
            _logger.LogInformation("Static data cache initialized successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initializing static data cache");
            throw;
        }
    }
}