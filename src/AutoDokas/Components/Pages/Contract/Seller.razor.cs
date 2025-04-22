using System.ComponentModel.DataAnnotations;
using AutoDokas.Components.Shared;
using AutoDokas.Data;
using AutoDokas.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AutoDokas.Components.Pages.Contract;

public partial class Seller : FormComponentBase<Seller.SellerFormModel>
{
    [Inject] private AppDbContext Context { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;

    [SupplyParameterFromForm(FormName = "SellerForm")]
    // Override the base Model property correctly
    protected override SellerFormModel Model { get => base.Model; set => base.Model = value; }

    [Parameter] public Guid? ContractId { get; set; }

    private bool isConsentChecked = false;

    protected override async Task OnInitializedAsync()
    {
        // Start by creating a new model instance
        Model = new SellerFormModel();

        if (ContractId.HasValue)
        {
            var entity = await Context.VehicleContracts.FindAsync(ContractId);
            if (entity is not null && entity.SellerInfo is not null)
            {
                // Map entity to form model
                Model = MapToFormModel(entity.SellerInfo);
            }
        }

        // Initialize the EditContext from the base class
        InitializeEditContext();
    }

    private SellerFormModel MapToFormModel(VehicleContract.PartyInfo partyInfo)
    {
        return new SellerFormModel
        {
            Name = partyInfo.Name ?? string.Empty,
            Email = partyInfo.Email ?? string.Empty,
            Code = partyInfo.Code ?? string.Empty,
            IsCompany = partyInfo.IsCompany,
            Phone = partyInfo.Phone ?? string.Empty,
            Address = partyInfo.Address ?? string.Empty
        };
    }

    private VehicleContract.PartyInfo MapToPartyInfo(SellerFormModel model)
    {
        return new VehicleContract.PartyInfo
        {
            Name = model.Name,
            Email = model.Email,
            Code = model.Code,
            IsCompany = model.IsCompany,
            Phone = model.Phone,
            Address = model.Address
        };
    }

    private async Task Submit()
    {
        try
        {
            Loading = true;

            VehicleContract entity;

            if (ValidateForm())
            {
                if (ContractId.HasValue)
                {
                    // Update existing entity
                    entity = await Context.VehicleContracts.FindAsync(ContractId.Value)
                        ?? new VehicleContract { Id = ContractId.Value };
                    entity.SellerInfo = MapToPartyInfo(Model);
                    Context.Update(entity);
                }
                else
                {
                    // Create new entity
                    entity = new VehicleContract
                    {
                        SellerInfo = MapToPartyInfo(Model)
                    };
                    await Context.AddAsync(entity);
                    await Context.SaveChangesAsync();
                    Navigation.NavigateTo($"/Vehicle/{entity.Id}");
                }
            }
            else
            {
                Loading = false;
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

    public class SellerFormModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Code is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Code must contain only digits")]
        public string Code { get; set; } = string.Empty;
        public bool IsCompany { get; set; } = false;

        [Required(ErrorMessage = "Phone is required")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Phone number must contain only digits")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        [StringLength(300, ErrorMessage = "Address cannot exceed 300 characters")]
        public string Address { get; set; } = string.Empty;
    }
}