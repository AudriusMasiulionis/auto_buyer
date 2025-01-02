using AutoDokas.Data;
using AutoDokas.Data.Models;
using Microsoft.AspNetCore.Components;

namespace AutoDokas.Components.Pages.Contract;

public partial class BuyerReview : ComponentBase
{
    [Inject] private AppDbContext Context { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    [Parameter] public Guid? ContractId { get; set; }
    private VehicleContract contract = null!;

    protected override async Task OnInitializedAsync()
    {
        contract = await Context.VehicleContracts.FindAsync(ContractId);
    }
}