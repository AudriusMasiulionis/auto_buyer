using AutoDokas.Data.Models;
using AutoDokas.Services.EmailTemplates;

namespace AutoDokas.Services;

/// <summary>
/// Service for creating and sending email notifications
/// </summary>
public class EmailNotificationService
{
    private readonly IEmailService _emailService;
    private readonly IEmailTemplateFactory _emailTemplateFactory;
    private readonly ILogger<EmailNotificationService> _logger;
    private readonly string _baseUrl;

    public EmailNotificationService(
        IEmailService emailService,
        IEmailTemplateFactory emailTemplateFactory,
        ILogger<EmailNotificationService> logger,
        IConfiguration configuration)
    {
        _emailService = emailService;
        _emailTemplateFactory = emailTemplateFactory;
        _logger = logger;
        _baseUrl = configuration["AWS:BaseUrl"] ?? "https://localhost";
    }

    /// <summary>
    /// Sends a contract notification email to the recipient
    /// </summary>
    /// <param name="to">Recipient email address</param>
    /// <param name="contract">The vehicle contract with details to include in the email</param>
    public async Task SendContractNotificationAsync(string to, VehicleContract contract)
    {
        try
        {
            var subject = $"Vehicle Contract: {contract.VehicleInfo?.Make} {contract.VehicleInfo?.RegistrationNumber}";
            
            // Create the email model
            var emailModel = new ContractCompletedEmailModel
            {
                Contract = contract,
                BaseUrl = _baseUrl
            };
            
            // Render the email template to HTML
            var htmlBody = await _emailTemplateFactory.RenderAsync(emailModel);
            
            // Send the email with the rendered HTML
            await _emailService.SendEmailAsync("noreply@autodokas.lt", to, subject, htmlBody);
            
            _logger.LogInformation("Contract notification email sent to {Recipient} for contract {ContractId}", 
                to, contract.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send contract notification email to {Recipient} for contract {ContractId}", 
                to, contract.Id);
            throw;
        }
    }
    
    // Additional email notification methods can be added here as needed
}