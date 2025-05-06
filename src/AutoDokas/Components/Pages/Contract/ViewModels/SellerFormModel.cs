using System.ComponentModel.DataAnnotations;
using AutoDokas.Data.Models;

namespace AutoDokas.Components.Pages.Contract.ViewModels;

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

    [Required(ErrorMessage = "Country is required")]
    public Country? Origin { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool HasConsented { get; set; } = false;

    public static SellerFormModel MapToFormModel(VehicleContract contract)
    {
        if (contract.SellerInfo == null)
        {
            throw new ArgumentNullException(nameof(contract.SellerInfo), "SellerInfo cannot be null");
        }

        var partyInfo = contract.SellerInfo;

        return new SellerFormModel
        {
            Name = partyInfo.Name ?? string.Empty,
            Email = partyInfo.Email ?? string.Empty,
            Code = partyInfo.Code ?? string.Empty,
            IsCompany = partyInfo.IsCompany,
            Phone = partyInfo.Phone ?? string.Empty,
            Address = partyInfo.Address ?? string.Empty,
            Origin = contract.Origin,
            CreatedAt = contract.CreatedAt,
            HasConsented = partyInfo.HasConsented
        };
    }

    public static VehicleContract MapToEntity(VehicleContract existing, SellerFormModel model)
    {
        existing.CreatedAt = model.CreatedAt;
        existing.Origin = model.Origin;
        existing.SellerInfo = new VehicleContract.PartyInfo
        {
            Name = model.Name,
            Email = model.Email,
            Code = model.Code,
            IsCompany = model.IsCompany,
            Phone = model.Phone,
            Address = model.Address,
            HasConsented = model.HasConsented
        };
        return existing;
    }
}