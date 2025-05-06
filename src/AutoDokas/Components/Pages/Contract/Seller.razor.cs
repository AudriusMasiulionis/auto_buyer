using AutoDokas.Components.Pages.Contract.ViewModels;
using AutoDokas.Components.Shared;
using AutoDokas.Data;
using AutoDokas.Data.Models;
using AutoDokas.Services;
using Microsoft.AspNetCore.Components;

namespace AutoDokas.Components.Pages.Contract;

public partial class Seller : FormComponentBase<SellerFormModel>
{
    [Inject] private AppDbContext Context { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    [Inject] private ICsvReader CsvReader { get; set; } = null!;

    [SupplyParameterFromForm(FormName = "SellerForm")]
    // Override the base Model property correctly
    protected override SellerFormModel Model { get => base.Model; set => base.Model = value; }

    [Parameter] public Guid? ContractId { get; set; }

    private List<Country> Countries { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        Countries = [.. await CsvReader.ReadAllAsync<Country>("countries.csv")];
        Model = new SellerFormModel();

        // Set default country to Lietuva (Lithuania)
        var lithuania = Countries.FirstOrDefault(c => c.Name == "Lietuva");
        if (lithuania != null)
        {
            Model.Origin = lithuania;
        }

        if (ContractId.HasValue)
        {
            var entity = await Context.VehicleContracts.FindAsync(ContractId);
            if (entity is not null && entity.SellerInfo is not null)
            {
                // Map entity to form model
                Model = SellerFormModel.MapToFormModel(entity);
            }
        }

        // Initialize the EditContext from the base class
        InitializeEditContext();
    }

    private async Task Submit()
    {
        try
        {
            Loading = true;

            VehicleContract entity;

            if (ContractId.HasValue)
            {
                // Update existing entity
                entity = await Context.VehicleContracts.FindAsync(ContractId.Value)
                    ?? new VehicleContract { Id = ContractId.Value };

                entity = SellerFormModel.MapToEntity(entity, Model);

                Context.Update(entity);
                await Context.SaveChangesAsync();

                Navigation.NavigateTo($"/Vehicle/{entity.Id}");
            }
            else
            {
                entity = SellerFormModel.MapToEntity(new VehicleContract(), Model);

                await Context.AddAsync(entity);
                await Context.SaveChangesAsync();
                
                Navigation.NavigateTo($"/Vehicle/{entity.Id}");
            }
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