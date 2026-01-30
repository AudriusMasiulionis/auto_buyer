using System.ComponentModel.DataAnnotations;
using AutoDokas.Data.Models;
using AutoDokas.Resources;

namespace AutoDokas.Components.Pages.Contract;

public class ContractViewModel
{
    // Buyer Information
    [Required(ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.NameRequired))]
    [StringLength(100, ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.NameMaxLength))]
    public string BuyerName { get; set; } = string.Empty;

    [Required(ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.EmailRequired))]
    [EmailAddress(ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.EmailInvalid))]
    [StringLength(100, ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.EmailMaxLength))]
    public string BuyerEmail { get; set; } = string.Empty;

    [Required(ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.CodeRequired))]
    [RegularExpression(@"^\d+$", ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.CodeDigitsOnly))]
    public string BuyerCode { get; set; } = string.Empty;

    [Required(ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.PhoneRequired))]
    [RegularExpression(@"^\d+$", ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.PhoneDigitsOnly))]
    public string BuyerPhone { get; set; } = string.Empty;

    [Required(ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.AddressRequired))]
    [StringLength(300, ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.AddressMaxLength))]
    public string BuyerAddress { get; set; } = string.Empty;

    public byte[]? SignatureData { get; set; }
    public byte[]? BuyerSignatureData { get; set; }

    // Contract Information
    [Required(ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.CountryRequired))]
    public Country? Origin { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool HasConsented { get; set; } = false;

    // Seller Information
    [Required(ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.NameRequired))]
    [StringLength(100, ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.NameMaxLength))]
    public string SellerName { get; set; } = string.Empty;

    [Required(ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.EmailRequired))]
    [EmailAddress(ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.EmailInvalid))]
    [StringLength(100, ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.EmailMaxLength))]
    public string SellerEmail { get; set; } = string.Empty;

    [Required(ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.CodeRequired))]
    [RegularExpression(@"^\d+$", ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.CodeDigitsOnly))]
    public string SellerCode { get; set; } = string.Empty;

    [Required(ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.PhoneRequired))]
    [RegularExpression(@"^\d+$", ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.PhoneDigitsOnly))]
    public string SellerPhone { get; set; } = string.Empty;

    [Required(ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.AddressRequired))]
    [StringLength(300, ErrorMessageResourceType = typeof(Text), ErrorMessageResourceName = nameof(Text.AddressMaxLength))]
    public string SellerAddress { get; set; } = string.Empty;

    // Vehicle Information
    [Required(ErrorMessage = "SDK yra privalomas")]
    public string VehicleSdk { get; set; } = string.Empty;

    [Required(ErrorMessage = "Markė yra privaloma")]
    public string VehicleMake { get; set; } = string.Empty;

    [Required(ErrorMessage = "Registracijos numeris yra privalomas")]
    public string VehicleRegistrationNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Rida yra privaloma")]
    [Range(0, int.MaxValue, ErrorMessage = "Rida turi būti teigiamas skaičius")]
    public int VehicleMillage { get; set; }

    [Required(ErrorMessage = "Identifikavimo numeris yra privalomas")]
    public string VehicleIdentificationNumber { get; set; } = string.Empty;

    public bool VehicleIsInspected { get; set; } = true;
    public bool VehicleHasBeenDamaged { get; set; }
    public bool VehiclePriorDamagesKnown { get; set; }
    public List<VehicleContract.Vehicle.Defect> VehicleDefects { get; set; } = [];

    // Payment Information
    [Required(ErrorMessage = "Kaina yra privaloma")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Kaina turi būti didesnė nei 0")]
    public decimal PaymentPrice { get; set; }

    [Required(ErrorMessage = "Pasirinkite mokėjimo būdą")]
    public VehicleContract.Payment.PaymentType? PaymentMethod { get; set; }

    public bool PaymentAtContractFormation { get; set; } = true;
    public DateOnly? PaymentDate { get; set; }
    public bool TransferInsurance { get; set; }
    public string? PaymentAdditionalInformation { get; set; }

    public static ContractViewModel FromContract(VehicleContract contract)
    {
        var model = new ContractViewModel
        {
            SignatureData = contract.SellerInfo?.SignatureData,
            BuyerSignatureData = contract.BuyerInfo?.SignatureData,
            Origin = contract.Origin,
            CreatedAt = contract.CreatedAt,
            HasConsented = contract.SellerInfo?.HasConsented ?? false
        };

        // Map Seller Info
        if (contract.SellerInfo != null)
        {
            model.SellerName = contract.SellerInfo.Name ?? string.Empty;
            model.SellerEmail = contract.SellerInfo.Email ?? string.Empty;
            model.SellerCode = contract.SellerInfo.Code ?? string.Empty;
            model.SellerPhone = contract.SellerInfo.Phone ?? string.Empty;
            model.SellerAddress = contract.SellerInfo.Address ?? string.Empty;
        }

        // Map Buyer Info
        if (contract.BuyerInfo != null)
        {
            model.BuyerName = contract.BuyerInfo.Name ?? string.Empty;
            model.BuyerEmail = contract.BuyerInfo.Email ?? string.Empty;
            model.BuyerCode = contract.BuyerInfo.Code ?? string.Empty;
            model.BuyerPhone = contract.BuyerInfo.Phone ?? string.Empty;
            model.BuyerAddress = contract.BuyerInfo.Address ?? string.Empty;
        }

        // Map Vehicle Info
        if (contract.VehicleInfo != null)
        {
            model.VehicleSdk = contract.VehicleInfo.Sdk ?? string.Empty;
            model.VehicleMake = contract.VehicleInfo.Make ?? string.Empty;
            model.VehicleRegistrationNumber = contract.VehicleInfo.RegistrationNumber ?? string.Empty;
            model.VehicleMillage = contract.VehicleInfo.Millage;
            model.VehicleIdentificationNumber = contract.VehicleInfo.IdentificationNumber ?? string.Empty;
            model.VehicleIsInspected = contract.VehicleInfo.IsInspected;
            model.VehicleHasBeenDamaged = contract.VehicleInfo.HasBeenDamaged;
            model.VehiclePriorDamagesKnown = contract.VehicleInfo.PriorDamagesKnown;
            model.VehicleDefects = contract.VehicleInfo.Defects?.ToList() ?? [];
        }

        // Map Payment Info
        if (contract.PaymentInfo != null)
        {
            model.PaymentPrice = contract.PaymentInfo.Price;
            model.PaymentMethod = contract.PaymentInfo.PaymentMethod;
            model.PaymentAtContractFormation = contract.PaymentInfo.PaymentAtContractFormation;
            model.PaymentDate = contract.PaymentInfo.PaymentDate;
            model.TransferInsurance = contract.PaymentInfo.TransferInsurance;
            model.PaymentAdditionalInformation = contract.PaymentInfo.AdditionalInformation;
        }

        return model;
    }

    public void ApplyToContract(VehicleContract contract)
    {
        // Update Contract Info
        contract.Origin = Origin;
        contract.CreatedAt = CreatedAt;

        // Update Seller Info
        contract.SellerInfo ??= new VehicleContract.PartyInfo();
        contract.SellerInfo.Name = SellerName;
        contract.SellerInfo.Email = SellerEmail;
        contract.SellerInfo.Code = SellerCode;
        contract.SellerInfo.Phone = SellerPhone;
        contract.SellerInfo.Address = SellerAddress;
        contract.SellerInfo.SignatureData = SignatureData;
        contract.SellerInfo.HasConsented = HasConsented;

        // Update Vehicle Info
        contract.VehicleInfo ??= new VehicleContract.Vehicle();
        contract.VehicleInfo.Sdk = VehicleSdk;
        contract.VehicleInfo.Make = VehicleMake;
        contract.VehicleInfo.RegistrationNumber = VehicleRegistrationNumber;
        contract.VehicleInfo.Millage = VehicleMillage;
        contract.VehicleInfo.IdentificationNumber = VehicleIdentificationNumber;
        contract.VehicleInfo.IsInspected = VehicleIsInspected;
        contract.VehicleInfo.HasBeenDamaged = VehicleHasBeenDamaged;
        contract.VehicleInfo.PriorDamagesKnown = VehiclePriorDamagesKnown;
        contract.VehicleInfo.Defects = VehicleDefects;

        // Update Payment Info
        contract.PaymentInfo ??= new VehicleContract.Payment();
        contract.PaymentInfo.Price = PaymentPrice;
        contract.PaymentInfo.PaymentMethod = PaymentMethod;
        contract.PaymentInfo.PaymentAtContractFormation = PaymentAtContractFormation;
        contract.PaymentInfo.PaymentDate = PaymentDate;
        contract.PaymentInfo.TransferInsurance = TransferInsurance;
        contract.PaymentInfo.AdditionalInformation = PaymentAdditionalInformation;

        // Update Buyer Info
        contract.BuyerInfo ??= new VehicleContract.PartyInfo();
        contract.BuyerInfo.Name = BuyerName;
        contract.BuyerInfo.Email = BuyerEmail;
        contract.BuyerInfo.Code = BuyerCode;
        contract.BuyerInfo.Phone = BuyerPhone;
        contract.BuyerInfo.Address = BuyerAddress;
        contract.BuyerInfo.SignatureData = BuyerSignatureData;
    }
}
