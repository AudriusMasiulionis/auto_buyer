using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AutoDokas.Components.Shared;

/// <summary>
/// A base class for form components that handles EditContext initialization, 
/// validation on field changes, and proper disposal.
/// </summary>
/// <typeparam name="TModel">The type of the model being edited</typeparam>
public abstract class FormComponentBase<TModel> : ComponentBase, IDisposable where TModel : class, new()
{
    protected EditContext EditContext = null!;
    protected ValidationMessageStore MessageStore = null!;
    protected virtual TModel Model { get; set; } = new();
    protected bool Loading { get; set; }
    
    // CSS class provider reference for direct manipulation
    protected BootstrapValidationFieldClassProvider CssProvider = null!;

    /// <summary>
    /// Initializes the EditContext with the model and sets up validation.
    /// Call this method from your component's OnInitialized or OnInitializedAsync method.
    /// </summary>
    protected virtual void InitializeEditContext()
    {
        EditContext = new EditContext(Model);
        MessageStore = new ValidationMessageStore(EditContext);
        
        // Create and store the CSS provider
        CssProvider = new BootstrapValidationFieldClassProvider();
        EditContext.SetFieldCssClassProvider(CssProvider);
        
        // Subscribe to field changed event to trigger validation on each input change
        EditContext.OnFieldChanged += FieldChanged;
    }

    /// <summary>
    /// Event handler that validates the form whenever a field value changes.
    /// </summary>
    protected virtual void FieldChanged(object? sender, FieldChangedEventArgs e)
    {
        // Validate the field that was changed
        EditContext.Validate();
        StateHasChanged();
    }
    

    /// <summary>
    /// Cleanup resources when the component is disposed.
    /// </summary>
    public virtual void Dispose()
    {
        if (EditContext != null)
        {
            EditContext.OnFieldChanged -= FieldChanged;
        }
    }
}