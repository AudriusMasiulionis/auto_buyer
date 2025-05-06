using AutoDokas.Data.Models;

namespace AutoDokas.Services;

/// <summary>
/// Interface for PDF generation services
/// </summary>
public interface IPdfService
{
    /// <summary>
    /// Generates a PDF document for a vehicle contract
    /// </summary>
    /// <param name="contract">The vehicle contract to generate a PDF for</param>
    /// <returns>The PDF document as a byte array</returns>
    Task<byte[]> GenerateContractPdfAsync(VehicleContract contract);
}