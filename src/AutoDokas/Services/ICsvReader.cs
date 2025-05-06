using System.Globalization;

namespace AutoDokas.Services;

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