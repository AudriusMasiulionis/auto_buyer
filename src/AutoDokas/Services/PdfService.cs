using AutoDokas.Components.Pdf;
using AutoDokas.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace AutoDokas.Services;

/// <summary>
/// Implementation of the PDF service using Blazor component rendering with Puppeteer
/// </summary>
public class PdfService : IPdfService
{
    private readonly ILogger<PdfService> _logger;
    private readonly HtmlRenderer _htmlRenderer;

    public PdfService(ILogger<PdfService> logger, HtmlRenderer htmlRenderer)
    {
        _logger = logger;
        _htmlRenderer = htmlRenderer;
    }

    /// <summary>
    /// Generates a PDF document for a vehicle contract
    /// </summary>
    public async Task<byte[]> GenerateContractPdfAsync(VehicleContract contract)
    {
        try
        {
            // First, render the contract component to HTML
            var html = await RenderContractComponentToHtmlAsync(contract);

            // Then convert the HTML to PDF using Puppeteer Sharp
            var pdfBytes = await ConvertHtmlToPdfAsync(html, contract);

            return pdfBytes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating PDF for contract {ContractId}", contract.Id);
            throw;
        }
    }

    /// <summary>
    /// Renders the Blazor component to HTML
    /// </summary>
    private async Task<string> RenderContractComponentToHtmlAsync(VehicleContract contract)
    {
        try
        {
            // Create parameters for the component
            var parameters = new Dictionary<string, object?>
            {
                { "Contract", contract }
            };

            // Render the contract component with the complete HTML document
            var html = await _htmlRenderer.Dispatcher.InvokeAsync(async () =>
            {
                var result = await _htmlRenderer.RenderComponentAsync<ContractPdfTemplate>(
                    ParameterView.FromDictionary(parameters));
                return result.ToHtmlString();
            });

            return html;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rendering component to HTML for contract {ContractId}", contract.Id);
            throw;
        }
    }

    /// <summary>
    /// Converts HTML to PDF using Puppeteer Sharp
    /// </summary>
    private async Task<byte[]> ConvertHtmlToPdfAsync(string html, VehicleContract contract)
    {
        try
        {
            // Initialize Puppeteer browser instance
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });

            // Create a new page
            await using var page = await browser.NewPageAsync();

            // Set content to our HTML
            await page.SetContentAsync(html);

            // Generate PDF
            var pdfOptions = new PdfOptions
            {
                Format = PaperFormat.A4,
                PrintBackground = true,
                DisplayHeaderFooter = true,
                MarginOptions = new MarginOptions
                {
                    Top = "20mm",
                    Bottom = "40mm",
                    Left = "10mm",
                    Right = "10mm"
                },
                HeaderTemplate = @"
        <div style='width: 100%; font-size: 10px; padding: 10px 20px; display: flex; justify-content: space-between;'>
            <div style='font-weight: bold;'>Auto Dokas</div>
            <div>Contract #<span class='pageNumber'></span> of <span class='totalPages'></span></div>
            <div>" + contract.VehicleInfo?.Make ?? "-" + @"</div>
        </div>",
                FooterTemplate = @"
        <div style='width: 100%; font-size: 10px; padding: 10px 20px; text-align: center;'>
            <div>Page <span class='pageNumber'></span> of <span class='totalPages'></span></div>
            <div>" + DateTime.Now.ToString("yyyy-MM-dd") + @"</div>
        </div>"

            };

            // Generate the PDF
            var pdfData = await page.PdfDataAsync(pdfOptions);

            return pdfData;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error converting HTML to PDF for contract {ContractId}", contract.Id);
            throw;
        }
    }
}