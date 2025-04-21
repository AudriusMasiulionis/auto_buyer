using System.ComponentModel.DataAnnotations;
using AutoDokas.Data;
using AutoDokas.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace AutoDokas.Components.Pages.Contract;

public partial class Seller : ComponentBase
{
    [Inject] private AppDbContext Context { get; set; } = null!;
    [Inject] private NavigationManager Navigation { get; set; } = null!;

    private EditContext _editContext = null!;

    [SupplyParameterFromForm(FormName = "SellerForm")]
    private SellerFormModel Model { get; set; } = new();

    [Parameter] public Guid? ContractId { get; set; }

    private bool _loading = false;

    protected override async Task OnInitializedAsync()
    {
        if (ContractId.HasValue)
        {
            var entity = await Context.VehicleContracts.FindAsync(ContractId);
            if (entity is not null && entity.SellerInfo is not null)
            {
                // Map entity to form model
                Model = MapToFormModel(entity.SellerInfo);
            }
        }

        _editContext = new EditContext(Model);
        _editContext.SetFieldCssClassProvider(new BootstrapValidationFieldClassProvider());
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
            _loading = true;

            VehicleContract entity;
            
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
            }
            
            await Context.SaveChangesAsync();
            Navigation.NavigateTo($"/Vehicle/{entity.Id}");
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

    class SellerFormModel
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

public class BootstrapValidationFieldClassProvider : FieldCssClassProvider
{
    public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
    {
        var isValid = editContext.IsValid(fieldIdentifier);
        var isModified = editContext.IsModified(fieldIdentifier);

        return $"{(isModified ? "modified " : "")}{(isValid ? "is-valid" : "is-invalid")}";
    }
}