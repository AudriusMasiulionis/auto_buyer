using AutoDokas.Data;
using AutoDokas.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AutoDokas.Components.Pages.Contract;

public partial class Buyer : ComponentBase
{
    [Inject] private AppDbContext Context { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;

    private EditContext _editContext = null!;

    [SupplyParameterFromForm(FormName = "BuyerForm")]
    private VehicleContract.PartyInfo Model { get; set; } = new();

    [Parameter] public Guid? ContractId { get; set; }

    private bool _loading = false;
    private VehicleContract? _entity;

    protected override async Task OnInitializedAsync()
    {
        if (ContractId.HasValue)
        {
            _entity = await Context.VehicleContracts.FindAsync(ContractId);
            if (_entity is not null)
            {
                Model = _entity.BuyerInfo;
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
            _entity.BuyerInfo.Code = Model.Code;
            _entity.BuyerInfo.IsCompany = Model.IsCompany;
            _entity.BuyerInfo.Phone = Model.Phone;
            _entity.BuyerInfo.Name = Model.Name;
            _entity.BuyerInfo.Address = Model.Address;

            Context.Update(_entity);
            await Context.SaveChangesAsync();
            Navigation.NavigateTo($"/BuyerReview/{_entity.Id}");
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