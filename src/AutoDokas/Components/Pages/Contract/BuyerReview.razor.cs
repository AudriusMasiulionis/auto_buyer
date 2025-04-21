using System.ComponentModel.DataAnnotations;
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
    private bool _loading = false;
    
    [SupplyParameterFromForm(FormName = "Form")]
    private BuyerReviewForm Model { get; set; } = new();
    
    // This field is bound to the SignatureField component
    private byte[]? signatureData;

    protected override async Task OnInitializedAsync()
    {
        if (ContractId.HasValue)
        {
            var foundContract = await Context.VehicleContracts.FindAsync(ContractId);
            if (foundContract != null)
            {
                contract = foundContract;
                
                // Initialize signature data from the contract if available
                if (contract.BuyerInfo?.SignatureData != null)
                {
                    signatureData = contract.BuyerInfo.SignatureData;
                }
            }
        }
    }
    
    private async Task Submit()
    {
        try
        {
            _loading = true;

            if (ContractId.HasValue)
            {
                // Store the signature data if available
                if (signatureData != null && contract.BuyerInfo != null)
                {
                    // Save signature to the buyer's info
                    contract.BuyerInfo.SignatureData = signatureData;
                }
                
                // Save changes to the database
                Context.VehicleContracts.Update(contract);
                await Context.SaveChangesAsync();
                
                // Navigate to confirmation page
                Navigation.NavigateTo($"/ContractCompleted");
            }
            else
            {
                // Fallback in case there is no ContractId
                Navigation.NavigateTo("/");
            }
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

    private class BuyerReviewForm
    {
        // Removed consent requirement since we removed the checkbox from the UI
    }
}