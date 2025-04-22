using AutoDokas.Data;
using AutoDokas.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AutoDokas.Components.Pages.Contract;

public partial class Buyer : ComponentBase
{
    [Inject] private AppDbContext Context { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;

    [SupplyParameterFromForm(FormName = "BuyerForm")]
    private VehicleContract.PartyInfo Model { get; set; } = new();

    [Parameter] public Guid? ContractId { get; set; }

    private VehicleContract? _entity;
    private bool _loading = false;
    private EditContext _editContext = null!;

    protected override async Task OnInitializedAsync()
    {
        _loading = true;
        
        if (ContractId.HasValue)
        {
            _entity = await Context.VehicleContracts.FindAsync(ContractId);
            if (_entity is not null && _entity.BuyerInfo is not null)
            {
                Model = _entity.BuyerInfo;
            }
        }
        else
        {
            _entity = new VehicleContract
            {
                Id = Guid.NewGuid(),
                BuyerInfo = new VehicleContract.PartyInfo()
            };
        }

        _editContext = new EditContext(Model);
        _loading = false;
    }

    private async Task Submit()
    {
        try
        {
            _loading = true;
            
            _entity.BuyerInfo = Model;

            if (ContractId.HasValue)
            {
                Context.Update(_entity);
            }
            else
            {
                await Context.AddAsync(_entity);
            }
            
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