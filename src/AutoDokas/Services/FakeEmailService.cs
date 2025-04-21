using System.Text;
using AutoDokas.Data.Models;
using Microsoft.Extensions.Logging;

namespace AutoDokas.Services;

/// <summary>
/// A fake implementation of the email service that logs emails rather than sending them.
/// This is intended for development and testing purposes.
/// </summary>
public class FakeEmailService : IEmailService
{
    private readonly ILogger<FakeEmailService> _logger;

    public FakeEmailService(ILogger<FakeEmailService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Logs the email instead of actually sending it
    /// </summary>
    public Task SendEmailAsync(string to, string subject, string body)
    {
        _logger.LogInformation("FAKE EMAIL SENT\n" +
                              "To: {To}\n" +
                              "Subject: {Subject}\n" +
                              "Body: {Body}", 
                              to, subject, body);
        
        return Task.CompletedTask;
    }

    /// <summary>
    /// Creates and logs a notification email about the contract
    /// </summary>
    public Task SendContractNotificationAsync(string to, VehicleContract contract)
    {
        var subject = $"Vehicle Contract: {contract.VehicleInfo?.Make} {contract.VehicleInfo?.RegistrationNumber}";
        var body = GenerateContractEmailBody(contract);
        
        return SendEmailAsync(to, subject, body);
    }

    /// <summary>
    /// Generates a formatted email body with contract details
    /// </summary>
    private string GenerateContractEmailBody(VehicleContract contract)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine("<h1>Vehicle Purchase Contract</h1>");
        sb.AppendLine("<p>A new vehicle purchase contract has been created for you to review.</p>");
        
        // Vehicle Information
        sb.AppendLine("<h2>Vehicle Details</h2>");
        sb.AppendLine("<ul>");
        sb.AppendLine($"<li><strong>Make:</strong> {contract.VehicleInfo?.Make}</li>");
        sb.AppendLine($"<li><strong>Registration Number:</strong> {contract.VehicleInfo?.RegistrationNumber}</li>");
        sb.AppendLine($"<li><strong>Identification Number:</strong> {contract.VehicleInfo?.IdentificationNumber}</li>");
        sb.AppendLine($"<li><strong>Mileage:</strong> {contract.VehicleInfo?.Millage} km</li>");
        sb.AppendLine("</ul>");
        
        // Seller Information
        sb.AppendLine("<h2>Seller Information</h2>");
        sb.AppendLine("<ul>");
        sb.AppendLine($"<li><strong>Name:</strong> {contract.SellerInfo?.Name}</li>");
        sb.AppendLine($"<li><strong>Email:</strong> {contract.SellerInfo?.Email}</li>");
        sb.AppendLine($"<li><strong>Phone:</strong> {contract.SellerInfo?.Phone}</li>");
        sb.AppendLine("</ul>");
        
        // Payment Information
        sb.AppendLine("<h2>Payment Details</h2>");
        sb.AppendLine("<ul>");
        sb.AppendLine($"<li><strong>Price:</strong> {contract.PaymentInfo?.Price} EUR</li>");
        sb.AppendLine($"<li><strong>Payment Method:</strong> {contract.PaymentInfo?.PaymentMethod}</li>");
        sb.AppendLine("</ul>");
        
        sb.AppendLine("<p>To view the full contract and complete the purchase, please follow this link: " +
                      $"<a href='https://localhost/seller/{contract.Id}'>View Contract</a></p>");
        
        sb.AppendLine("<p>Thank you for using AutoDokas!</p>");
        
        return sb.ToString();
    }
}