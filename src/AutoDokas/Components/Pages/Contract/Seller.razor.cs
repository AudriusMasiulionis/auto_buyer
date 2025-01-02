using AutoDokas.Data;
using AutoDokas.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AutoDokas.Components.Pages.Contract;

public partial class Seller : ComponentBase
{
    [Inject] private AppDbContext Context { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;

    private EditContext _editContext = null!;

    [SupplyParameterFromForm(FormName = "SellerForm")]
    private VehicleContract.PartyInfo Model { get; set; } = new();

    [Parameter] public Guid? ContractId { get; set; }

    private bool _loading = false;

    protected override async Task OnInitializedAsync()
    {
        if (ContractId.HasValue)
        {
            var entity = await Context.VehicleContracts.FindAsync(ContractId);
            if (entity is not null)
            {
                Model = entity.SellerInfo;
            }
        }

        _editContext = new EditContext(Model);
        _editContext.SetFieldCssClassProvider(new BootstrapValidationFieldClassProvider());
    }


    private async Task Submit()
    {
        try
        {
            _loading = true;

            // Create the entity and save it to the database
            var entity = new VehicleContract
            {
                SellerInfo = Model
            };
            await Context.AddAsync(entity);
            await Context.SaveChangesAsync();
            Navigation.NavigateTo($"/Vehicle/{entity.Id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
        finally
        {
            _loading = false;
        }
    }
}

public class BootstrapValidationFieldClassProvider : FieldCssClassProvider
{
    public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
    {
        var isValid = editContext.IsValid(fieldIdentifier);
        var isModified = editContext.IsModified(fieldIdentifier);

        // Blazor vs. Bootstrap:
        // isvalid = is-valid
        // isinvalid = is-invalid

        return $"{(isModified ? "modified " : "")}{(isValid ? "is-valid123" : "is-invalid123")}";
    }
}