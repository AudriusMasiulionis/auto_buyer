using AutoDokas.Data.Models;
using AutoDokas.Extensions;

namespace AutoDokas.Services;

/// <summary>
/// Service for creating and sending email notifications
/// </summary>
public class EmailNotificationService
{
    private readonly IEmailService _emailService;
    private readonly ILogger<EmailNotificationService> _logger;
    private readonly string _fromEmail;
    private readonly string _baseUrl;
    private readonly string? _toOverride;

    private static readonly Lazy<string> ContractCompletedHtml = new(() =>
        LoadTemplate("ContractCompletedEmail.html"));

    private static readonly Lazy<string> BuyerInviteHtml = new(() =>
        LoadTemplate("BuyerInviteInformationFill.html"));

    public EmailNotificationService(
        IEmailService emailService,
        ILogger<EmailNotificationService> logger,
        IConfiguration configuration)
    {
        _emailService = emailService;
        _logger = logger;
        _fromEmail = configuration["Email:FromAddress"] ?? "noreply@autodokas.lt";
        _baseUrl = configuration["Email:BaseUrl"] ?? "https://localhost";
        var toOverride = configuration["Email:ToOverride"];
        _toOverride = string.IsNullOrWhiteSpace(toOverride) ? null : toOverride;
    }

    public async Task SendContractNotificationAsync(string to, VehicleContract contract)
    {
        var recipient = _toOverride ?? to;

        try
        {
            var vars = BuildCommonVars(contract);
            vars["buyer_name"] = contract.BuyerInfo?.Name ?? "-";
            vars["contract_date"] = contract.CreatedAt.ToShortDateString();
            vars["download_url"] = $"{_baseUrl}/contract/download/{contract.Id}";

            await _emailService.SendEmailAsync(
                _fromEmail,
                recipient,
                "Transporto priemonės sutartis užpildyta",
                ContractCompletedHtml.Value,
                vars);

            _logger.LogInformation("ContractCompleted email sent to {Recipient} for contract {ContractId}",
                recipient, contract.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send ContractCompleted email to {Recipient} for contract {ContractId}",
                recipient, contract.Id);
            throw;
        }
    }

    public async Task SendBuyerInviteAsync(string to, VehicleContract contract)
    {
        var recipient = _toOverride ?? to;

        try
        {
            var vars = BuildCommonVars(contract);
            vars["buyer_url"] = $"{_baseUrl}/buyer/{contract.Id}";

            await _emailService.SendEmailAsync(
                _fromEmail,
                recipient,
                "Pirkėjo informacijos pildymas",
                BuyerInviteHtml.Value,
                vars);

            _logger.LogInformation("BuyerInvite email sent to {Recipient} for contract {ContractId}",
                recipient, contract.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send BuyerInvite email to {Recipient} for contract {ContractId}",
                recipient, contract.Id);
            throw;
        }
    }

    private Dictionary<string, string> BuildCommonVars(VehicleContract contract)
    {
        return new Dictionary<string, string>
        {
            ["vehicle_make"] = contract.VehicleInfo?.Make ?? "-",
            ["vehicle_reg"] = contract.VehicleInfo?.RegistrationNumber ?? "-",
            ["vehicle_vin"] = contract.VehicleInfo?.IdentificationNumber ?? "-",
            ["vehicle_mileage"] = contract.VehicleInfo?.Millage.ToString() ?? "0",
            ["seller_name"] = contract.SellerInfo?.Name ?? "-",
            ["price"] = contract.PaymentInfo?.Price.ToString("N2") ?? "0.00",
            ["payment_method"] = contract.PaymentInfo?.PaymentMethod?.GetDisplayName() ?? "-"
        };
    }

    private static string LoadTemplate(string fileName)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Components", "Email", fileName);
        return File.ReadAllText(path);
    }
}
