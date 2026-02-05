using System.Globalization;
using CsvHelper.Configuration;
using Microsoft.Extensions.Caching.Memory;

namespace AutoDokas.Services;

/// <summary>
/// Implementation of the ICsvDataService interface for reading CSV files in the StaticData folder
/// </summary>
public class CsvReader : ICsvReader
{
    private readonly string _staticDataPath;

    /// <summary>
    /// Initializes a new instance of the CsvDataService class
    /// </summary>
    /// <param name="environment">The web host environment</param>
    public CsvReader(IWebHostEnvironment environment)
    {
        _staticDataPath = Path.Combine(environment.ContentRootPath, "Data", "StaticData");
    }

    /// <inheritdoc />
    public async Task<IEnumerable<T>> ReadAllAsync<T>(string filePath)
    {
        return await ReadAllAsync<T>(filePath, CultureInfo.InvariantCulture);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<T>> ReadAllAsync<T>(string filePath, CultureInfo culture)
    {
        var fullPath = Path.Combine(_staticDataPath, filePath);
        
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"CSV file not found: {filePath}", fullPath);
        }

        using var reader = new StreamReader(fullPath);
        using var csv = new CsvHelper.CsvReader(reader, new CsvConfiguration(culture)
        {
            HeaderValidated = null,
            MissingFieldFound = null
        });
        
        var records = new List<T>();
        await foreach (var record in csv.GetRecordsAsync<T>())
        {
            records.Add(record);
        }
        
        return records;
    }
}

/// <summary>
/// Service for reading data from CSV files
/// </summary>
public interface ICsvReader
{
    /// <summary>
    /// Reads all records from a CSV file and returns them as a collection of the specified type
    /// </summary>
    /// <typeparam name="T">The type of records to return</typeparam>
    /// <param name="filePath">The path to the CSV file relative to the StaticData folder</param>
    /// <returns>A collection of records of type T</returns>
    Task<IEnumerable<T>> ReadAllAsync<T>(string filePath);
    
    /// <summary>
    /// Reads all records from a CSV file with the specified culture and returns them as a collection of the specified type
    /// </summary>
    /// <typeparam name="T">The type of records to return</typeparam>
    /// <param name="filePath">The path to the CSV file relative to the StaticData folder</param>
    /// <param name="culture">The culture to use when reading the CSV file</param>
    /// <returns>A collection of records of type T</returns>
    Task<IEnumerable<T>> ReadAllAsync<T>(string filePath, CultureInfo culture);
}

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