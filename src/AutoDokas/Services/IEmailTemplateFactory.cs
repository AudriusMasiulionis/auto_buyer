namespace AutoDokas.Services;

/// <summary>
/// Service for generating email content from templates
/// </summary>
public interface IEmailTemplateFactory
{
    /// <summary>
    /// Renders a template to HTML using the provided model
    /// </summary>
    /// <typeparam name="TModel">The type of the model for the template</typeparam>
    /// <param name="model">The model data to pass to the template</param>
    /// <returns>The rendered HTML content</returns>
    Task<string> RenderAsync<TModel>(TModel model);
}