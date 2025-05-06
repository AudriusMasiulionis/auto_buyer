using System.Diagnostics;
using System.Reflection;
using AutoDokas.Components.Email;
using AutoDokas.Services.EmailTemplates;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace AutoDokas.Services;

/// <summary>
/// Factory for generating email content from Razor components
/// </summary>
public class RazorEmailTemplateFactory : IEmailTemplateFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly HtmlRenderer _htmlRenderer;
    private readonly ILogger<RazorEmailTemplateFactory> _logger;

    public RazorEmailTemplateFactory(
        IServiceProvider serviceProvider,
        HtmlRenderer htmlRenderer,
        ILogger<RazorEmailTemplateFactory> logger)
    {
        _serviceProvider = serviceProvider;
        _htmlRenderer = htmlRenderer;
        _logger = logger;
    }

    /// <summary>
    /// Renders an email template to HTML string
    /// </summary>
    public async Task<string> RenderAsync<TModel>(TModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        
        try
        {
            // Map the model type to a component type
            Type componentType = GetComponentTypeForModel(model);
            
            // Create the component parameters with the model
            var parameters = new Dictionary<string, object?>
            {
                { "Model", model }
            };

            // Render the component to HTML
            var html = await _htmlRenderer.Dispatcher.InvokeAsync(async () =>
            {
                var output = await _htmlRenderer.RenderComponentAsync(
                    componentType,
                    ParameterView.FromDictionary(parameters));
                
                return output.ToHtmlString();
            });

            return html;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to render email template for model type {ModelType}", typeof(TModel).Name);
            throw;
        }
    }

    /// <summary>
    /// Maps a model type to the appropriate component type
    /// </summary>
    private Type GetComponentTypeForModel<TModel>(TModel model)
    {
        return model switch
        {
            ContractCompletedEmailModel => typeof(ContractCompletedEmail),
            // Add more model-to-component mappings here as needed
            _ => throw new ArgumentException($"No template component found for model type: {typeof(TModel).Name}")
        };
    }
}