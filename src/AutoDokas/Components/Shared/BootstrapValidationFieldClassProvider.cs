using Microsoft.AspNetCore.Components.Forms;

namespace AutoDokas.Components.Shared;

public class BootstrapValidationFieldClassProvider : FieldCssClassProvider
{
    // Flag to indicate form was submitted
    private bool _formSubmitted;
    
    // Method to mark the form as submitted, called from components
    public void MarkAsSubmitted()
    {
        _formSubmitted = true;
    }
    
    public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
    {
        // Check if the field is a checkbox based on property type
        bool isCheckbox = IsCheckboxField(fieldIdentifier);
        
        // Use the appropriate base class depending on the input type
        var cssClass = isCheckbox ? "form-check-input" : "form-control";
        

        // todo form is not marked as submited so thats why no bootstrap validation classes are applied
        // After form submission, always show validation state regardless of whether field was modified
        if (_formSubmitted || editContext.IsModified(fieldIdentifier))
        {
            var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();
            cssClass += isValid ? " is-valid" : " is-invalid";
        }
        
        return cssClass;
    }
    
    private bool IsCheckboxField(FieldIdentifier fieldIdentifier)
    {
        // Best way to detect checkbox: check if the property type is boolean
        if (fieldIdentifier.Model != null)
        {
            var propertyInfo = fieldIdentifier.Model.GetType().GetProperty(fieldIdentifier.FieldName);
            if (propertyInfo != null)
            {
                // If it's a boolean property, it's almost certainly a checkbox
                return propertyInfo.PropertyType == typeof(bool);
            }
        }
        
        return false;
    }
}