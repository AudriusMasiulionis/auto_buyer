using System.ComponentModel.DataAnnotations;
using AutoDokas.Data.Models;

namespace AutoDokas.Components.Pages.Contract.ViewModels;

public class VehicleViewModel
{
    [Required(ErrorMessage = "Please enter the vehicle Sdk.")]
    public string Sdk { get; set; } = string.Empty;
    [Required(ErrorMessage = "Please enter the vehicle make.")]
    public string Make { get; set; } = string.Empty;
    [Required(ErrorMessage = "Please enter the vehicle registration number.")]
    public string RegistrationNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter the vehicle millage.")]
    [Range(0, int.MaxValue, ErrorMessage = "Millage must be a positive number.")]
    public int Millage { get; set; }

    [Required(ErrorMessage = "Please enter the vehicle identification number.")]
    public string IdentificationNumber { get; set; } = string.Empty;
    public bool IsInspected { get; set; } = true;
    public bool HasBeenDamaged { get; set; }
    public bool PriorDamagesKnown { get; set; }
    public List<VehicleContract.Vehicle.Defect> Defects { get; set; } = [];
    public string? AdditionalInformation { get; set; }

    // Mapping methods
    public static VehicleViewModel FromEntity(VehicleContract.Vehicle entity)
    {
        if (entity == null)
            return new VehicleViewModel();

        return new VehicleViewModel
        {
            Sdk = entity.Sdk,
            Make = entity.Make,
            RegistrationNumber = entity.RegistrationNumber,
            Millage = entity.Millage,
            IdentificationNumber = entity.IdentificationNumber,
            IsInspected = entity.IsInspected,
            HasBeenDamaged = entity.HasBeenDamaged,
            PriorDamagesKnown = entity.PriorDamagesKnown,
            Defects = entity.Defects?.ToList() ?? [],
            AdditionalInformation = entity.AdditionalInformation
        };
    }

    public VehicleContract.Vehicle ToEntity()
    {
        return new VehicleContract.Vehicle
        {
            Sdk = Sdk,
            Make = Make,
            RegistrationNumber = RegistrationNumber,
            Millage = Millage,
            IdentificationNumber = IdentificationNumber,
            IsInspected = IsInspected,
            HasBeenDamaged = HasBeenDamaged,
            PriorDamagesKnown = PriorDamagesKnown,
            Defects = Defects,
            AdditionalInformation = AdditionalInformation
        };
    }
}