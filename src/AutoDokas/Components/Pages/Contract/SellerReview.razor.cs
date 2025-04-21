using System.ComponentModel.DataAnnotations;
using AutoDokas.Data;
using AutoDokas.Data.Models;
using AutoDokas.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AutoDokas.Components.Pages.Contract;

public partial class SellerReview : ComponentBase
{
    [Inject] private AppDbContext Context { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    [Inject] private IEmailService EmailService { get; set; } = null!;
    [Parameter] public Guid? ContractId { get; set; }
    private VehicleContract contract = new();
    private bool _loading = false;
    
    [SupplyParameterFromForm(FormName = "Form")]
    private SellerReviewForm Model { get; set; } = new();
    
    protected override async Task OnInitializedAsync()
    {
        if (ContractId.HasValue)
        {
            var foundContract = await Context.VehicleContracts.FindAsync(ContractId);
            if (foundContract != null)
            {
                contract = foundContract;

                Model.BuyerEmail = contract.BuyerInfo?.Email ?? string.Empty;
                Model.SignatureData = contract.SellerInfo?.SignatureData ?? null;
            }
        }
    }
    
    private async Task Submit()
    {
        try
        {
            _loading = true;

            // Send the email to the buyer using our email service
            if (ContractId.HasValue)
            {
                contract.BuyerInfo ??= new VehicleContract.PartyInfo();
                
                contract.BuyerInfo.Email = Model.BuyerEmail;
                
                // Store the signature data if available
                if (Model.SignatureData != null && contract.SellerInfo != null)
                {
                    // Save signature to the seller's info
                    contract.SellerInfo.SignatureData = Model.SignatureData;
                }
                
                // Save changes to the database
                Context.VehicleContracts.Update(contract);
                await Context.SaveChangesAsync();
                
                // Send the email with contract details
                await EmailService.SendContractNotificationAsync(Model.BuyerEmail, contract);
                
                // Navigate to confirmation page
                Navigation.NavigateTo($"/BuyerNotificationSent");
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

    private class SellerReviewForm
    {
        [Required(ErrorMessage = "Buyer email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string BuyerEmail { get; set; } = string.Empty;
        
        public byte[]? SignatureData { get; internal set; }
    }
}