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

    protected override async Task OnInitializedAsync()
    {
        _loading = true;

        _contract = await Context.VehicleContracts.FindAsync(ContractId);
        if (_contract?.PaymentInfo != null)
        {
            Model = _contract.PaymentInfo;
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