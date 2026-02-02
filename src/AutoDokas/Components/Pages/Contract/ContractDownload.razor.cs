using System.ComponentModel.DataAnnotations;
using AutoDokas.Data;
using AutoDokas.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AutoDokas.Components.Pages.Contract;

public partial class ContractDownload : ComponentBase
{
    [Parameter] public Guid ContractId { get; set; }
    [Inject] private AppDbContext Context { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;
    
    private VehicleContract? contract;
    private bool loading = true;
    private bool isVerified = false;
    private bool showEmailError = false;
    
    private VerificationModel verificationModel = new();
    
    protected override async Task OnInitializedAsync()
    {
        await LoadContract();
    }

    private async Task LoadContract()
    {
        loading = true;
        try
        {
            contract = await Context.VehicleContracts.FindAsync(ContractId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading contract: {ex.Message}");
        }
        finally
        {
            loading = false;
        }
    }
    
    private Task VerifyEmail()
    {
        showEmailError = false;
        
        // Check if the email matches the seller or buyer email
        if (contract?.SellerInfo?.Email == verificationModel.Email || 
            contract?.BuyerInfo?.Email == verificationModel.Email)
        {
            isVerified = true;
        }
        else
        {
            showEmailError = true;
        }

        return Task.CompletedTask;
    }
    
    private string GetDownloadUrl()
    {
        string baseUri = Navigation.BaseUri.TrimEnd('/');
        return $"{baseUri}/api/contracts/{ContractId}/download?email={Uri.EscapeDataString(verificationModel.Email)}";
    }
    
    public class VerificationModel
    {
        [Required(ErrorMessage = "El. pašto adresas yra privalomas")]
        [EmailAddress(ErrorMessage = "Įveskite teisingą el. pašto adresą")]
        public string Email { get; set; } = string.Empty;
    }
}