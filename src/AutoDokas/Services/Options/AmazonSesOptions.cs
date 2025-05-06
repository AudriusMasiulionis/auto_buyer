namespace AutoDokas.Services.Options;

/// <summary>
/// Configuration options for Amazon SES email service
/// </summary>
public class AmazonSesOptions
{
    /// <summary>
    /// The section name in the configuration file
    /// </summary>
    public const string SectionName = "AWS";
    
    /// <summary>
    /// AWS region name (e.g., "eu-west-1")
    /// </summary>
    public string Region { get; set; } = "eu-west-1";
}