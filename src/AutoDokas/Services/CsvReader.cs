using System.Globalization;
using CsvHelper.Configuration;

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