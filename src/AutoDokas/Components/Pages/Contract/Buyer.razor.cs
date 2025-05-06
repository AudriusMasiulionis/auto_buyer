using AutoDokas.Components.Shared;
using AutoDokas.Data;
using AutoDokas.Data.Models;
using Microsoft.AspNetCore.Components;

namespace AutoDokas.Components.Pages.Contract;

public partial class Buyer : FormComponentBase<VehicleContract.PartyInfo>
{
    [Inject] private AppDbContext Context { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;

    [SupplyParameterFromForm(FormName = "BuyerForm")]
    protected override VehicleContract.PartyInfo Model { get => base.Model; set => base.Model = value; }

    [Parameter] public Guid? ContractId { get; set; }

    private VehicleContract? _entity;

    protected override async Task OnInitializedAsync()
    {
        // Start by creating a new model instance
        Model = new VehicleContract.PartyInfo();
        
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
                BuyerInfo = Model
            };
        }

        // Initialize the EditContext from the base class
        InitializeEditContext();
    }

    private async Task Submit()
    {
        try
        {
            Loading = true;
            
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
            Loading = false;
        }
    }
}