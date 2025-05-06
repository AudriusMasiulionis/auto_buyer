using AutoDokas.Data.Models;

namespace AutoDokas.Services.EmailTemplates;

/// <summary>
/// Model for generating an email when a contract is completed
/// </summary>
public class ContractCompletedEmailModel
{
    /// <summary>
    /// The contract that was completed
    /// </summary>
    public VehicleContract Contract { get; set; } = null!;
    
    /// <summary>
    /// The application base URL for generating links
    /// </summary>
    public string BaseUrl { get; set; } = "https://localhost";
}