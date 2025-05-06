using AutoDokas.Data;
using AutoDokas.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AutoDokas.Components.Pages.Contract;

public partial class Payment : ComponentBase
{
    [Inject] private AppDbContext Context { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    private EditContext _editContext = null!;
    private VehicleContract.Payment Model { get; set; } = new();
    [Parameter] public Guid ContractId { get; set; }
    private VehicleContract _contract;
    private bool _loading = false;
    
    // Property to bind with the CreatedAt datepicker
    private DateTime ContractCreatedAt { get; set; } = DateTime.UtcNow;

    protected override async Task OnInitializedAsync()
    {
        _loading = true;

        _contract = await Context.VehicleContracts.FindAsync(ContractId);
        if (_contract?.PaymentInfo != null)
        {
            Model = _contract.PaymentInfo;
        }
        
        // Initialize the datepicker with the current contract creation date
        if (_contract != null)
        {
            ContractCreatedAt = _contract.CreatedAt;
        }

        _editContext = new EditContext(Model);
        
        _loading = false;
    }

    private async Task Submit()
    {
        try
        {
            _loading = true;
            _contract.PaymentInfo = Model;
            
            // Update the contract's CreatedAt property with the selected date
            _contract.CreatedAt = ContractCreatedAt;
            
            Context.VehicleContracts.Update(_contract);
            await Context.SaveChangesAsync();
            Navigation.NavigateTo($"/SellerReview/{_contract.Id}");
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