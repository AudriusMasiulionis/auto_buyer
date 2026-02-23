using AutoDokas.Data;
using AutoDokas.Services;

namespace AutoDokas.Api;

/// <summary>
/// Contract API endpoints
/// </summary>
public static class ContractEndpoints
{
    /// <summary>
    /// Maps all contract-related API endpoints
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    public static void MapContractEndpoints(this IEndpointRouteBuilder endpoints)
    {
        // Contract download API endpoint
        endpoints.MapGet("/api/contracts/{id}/download", DownloadContractAsync);
    }

    /// <summary>
    /// Handles the contract download request
    /// </summary>
    private static async Task<IResult> DownloadContractAsync(
        Guid id,
        string? email,
        AppDbContext context,
        PdfService pdfService,
        ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("AutoDokas.Api.ContractEndpoints");

        // Find the contract
        var contract = await context.VehicleContracts.FindAsync(id);
        if (contract == null)
        {
            logger.LogWarning("Contract {ContractId} not found", id);
            return Results.NotFound("Contract not found");
        }

        // Verify email if provided
        if (!string.IsNullOrEmpty(email))
        {
            // Check if the email matches either seller or buyer
            bool isAuthorized = (contract.SellerInfo?.Email == email) ||
                                (contract.BuyerInfo?.Email == email);

            if (!isAuthorized)
            {
                logger.LogWarning("Unauthorized download attempt for contract {ContractId}", id);
                return Results.Unauthorized();
            }
        }

        // Generate the PDF
        byte[] pdfBytes = await pdfService.GenerateContractPdfAsync(contract);

        logger.LogInformation("Contract {ContractId} downloaded successfully", id);

        // Return the PDF as a downloadable file
        string fileName = $"pirkimo-pardavimo-sutartis-{contract.VehicleInfo?.RegistrationNumber}.pdf";
        return Results.File(pdfBytes, "application/pdf", fileName);
    }
}