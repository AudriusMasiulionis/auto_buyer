using AutoDokas.Data;
using AutoDokas.Data.Models;
using AutoDokas.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

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
        IPdfService pdfService)
    {
        // Find the contract
        var contract = await context.VehicleContracts.FindAsync(id);
        if (contract == null)
        {
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
                return Results.Unauthorized();
            }
        }

        // Generate the PDF
        byte[] pdfBytes = await pdfService.GenerateContractPdfAsync(contract);

        // Return the PDF as a downloadable file
        string fileName = $"vehicle-contract-{contract.VehicleInfo?.RegistrationNumber ?? id.ToString()}.pdf";
        return Results.File(pdfBytes, "application/pdf", fileName);
    }
}