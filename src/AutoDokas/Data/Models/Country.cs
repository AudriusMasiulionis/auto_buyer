namespace AutoDokas.Data.Models;

/// <summary>
/// Represents a country entity with code and name in different languages
/// </summary>
public class Country
{
    /// <summary>
    /// Gets or sets the English name of the country
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the two-letter country code (ISO 3166-1 alpha-2)
    /// </summary>
    public string Code { get; set; } = string.Empty;
}