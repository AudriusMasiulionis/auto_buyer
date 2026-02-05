using AutoDokas.Data.Models;

namespace AutoDokas.Services.EmailTemplates;

/// <summary>
/// Model for generating an email when a buyer is invited to fill information
/// </summary>
public class BuyerInviteInformationFillModel : IEmailModel
{
    public string Subject => "Pirkėjo informacijos pildymas";

    /// <summary>
    /// The contract for which buyer information is needed
    /// </summary>
    public VehicleContract Contract { get; set; } = null!;

    /// <summary>
    /// The application base URL for generating links
    /// </summary>
    public string BaseUrl { get; set; } = "https://localhost";
}