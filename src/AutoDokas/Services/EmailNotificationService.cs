using AutoDokas.Data.Models;
using AutoDokas.Services.EmailTemplates;

namespace AutoDokas.Services;

/// <summary>
/// Service for creating and sending email notifications
/// </summary>
public class EmailNotificationService
{
    private readonly IEmailService _emailService;
    private readonly RazorEmailTemplateFactory _emailTemplateFactory;
    private readonly ILogger<EmailNotificationService> _logger;
    private readonly string _fromEmail;
    private readonly string _baseUrl;
    private readonly string? _toOverride;

    public EmailNotificationService(
        IEmailService emailService,
        RazorEmailTemplateFactory emailTemplateFactory,
        ILogger<EmailNotificationService> logger,
        IConfiguration configuration)
    {
        _emailService = emailService;
        _emailTemplateFactory = emailTemplateFactory;
        _logger = logger;
        _fromEmail = configuration["Email:FromAddress"] ?? "noreply@autodokas.lt";
        _baseUrl = configuration["Email:BaseUrl"] ?? "https://localhost";
        var toOverride = configuration["Email:ToOverride"];
        _toOverride = string.IsNullOrWhiteSpace(toOverride) ? null : toOverride;
    }

    public async Task SendContractNotificationAsync(string to, VehicleContract contract)
    {
        await SendAsync<ContractCompletedEmailModel>(to, contract);
    }

    public async Task SendBuyerInviteAsync(string to, VehicleContract contract)
    {
        await SendAsync<BuyerInviteInformationFillModel>(to, contract);
    }

    private async Task SendAsync<TModel>(string to, VehicleContract contract) where TModel : IEmailModel, new()
    {
        var recipient = _toOverride ?? to;

        try
        {
            var model = new TModel
            {
                Contract = contract,
                BaseUrl = _baseUrl
            };

            var rendered = await _emailTemplateFactory.RenderAsync(model);

            await _emailService.SendEmailAsync(_fromEmail, recipient, rendered.Subject, rendered.Body);

            _logger.LogInformation("{EmailType} sent to {Recipient} for contract {ContractId}",
                typeof(TModel).Name, recipient, contract.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send {EmailType} to {Recipient} for contract {ContractId}",
                typeof(TModel).Name, recipient, contract.Id);
            throw;
        }
    }
}
