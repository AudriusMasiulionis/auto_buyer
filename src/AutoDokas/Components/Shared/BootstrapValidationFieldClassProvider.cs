using Microsoft.AspNetCore.Components.Forms;

namespace AutoDokas.Components.Shared;

public class BootstrapValidationFieldClassProvider : FieldCssClassProvider
{
    public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
    {
        bool isCheckbox = IsCheckboxField(fieldIdentifier);
        var cssClass = isCheckbox ? "form-check-input" : "form-control";

        var hasMessages = editContext.GetValidationMessages(fieldIdentifier).Any();

        // Show validation state if field was modified OR has validation errors
        if (editContext.IsModified(fieldIdentifier) || hasMessages)
        {
            cssClass += hasMessages ? " is-invalid" : " is-valid";
        }

        return cssClass;
    }

    private static bool IsCheckboxField(FieldIdentifier fieldIdentifier)
    {
        if (fieldIdentifier.Model == null) return false;

        var propertyInfo = fieldIdentifier.Model.GetType().GetProperty(fieldIdentifier.FieldName);
        return propertyInfo?.PropertyType == typeof(bool);
    }
}