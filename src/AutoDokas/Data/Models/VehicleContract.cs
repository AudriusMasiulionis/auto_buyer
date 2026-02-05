using System.ComponentModel.DataAnnotations;
using AutoDokas.Resources;

namespace AutoDokas.Data.Models;

public class VehicleContract
{
    public Guid Id { get; set; }
    public PartyInfo? SellerInfo { get; set; }
    public PartyInfo? BuyerInfo { get; set; }
    public Vehicle? VehicleInfo { get; set; }
    public Payment? PaymentInfo { get; set; }
    public Country? Origin { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Form section progress
    public FormSectionStatus VehicleStatus { get; set; }
    public FormSectionStatus PaymentStatus { get; set; }
    public FormSectionStatus SellerStatus { get; set; }
    public FormSectionStatus BuyerMethodStatus { get; set; }
    public FormSectionStatus BuyerInfoStatus { get; set; }
    public FormSectionStatus FinalStatus { get; set; }

    public enum FormSectionStatus
    {
        NotStarted,
        InProgress,
        Completed
    }

    public class PartyInfo
    {
        public string? Code { get; set; }
        public string? Phone { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public byte[]? SignatureData { get; set; }
        public bool HasConsented { get; set; }
    }

    public class Vehicle
    {
        public string Sdk { get; set; }
        public string Make { get; set; }
        public string RegistrationNumber { get; set; }
        public string? RegistrationCertificate { get; set; }
        public int Millage { get; set; }
        public string IdentificationNumber { get; set; }
        public bool IsInspected { get; set; }
        public bool HasBeenDamaged { get; set; }
        public bool DamagedDuringOwnership { get; set; }
        public bool DamageIncidentsKnown { get; set; }
        public List<Defect> Defects { get; set; } = [];
        public string? AdditionalInformation { get; set; }

        public enum Defect
        {
            [Display(Name = "Defect_BrakeSystem", ResourceType = typeof(Text))]
            BrakeSystem,
            [Display(Name = "Defect_SteeringSuspension", ResourceType = typeof(Text))]
            SteeringSuspension,
            [Display(Name = "Defect_Lighting", ResourceType = typeof(Text))]
            Lighting,
            [Display(Name = "Defect_SafetySystems", ResourceType = typeof(Text))]
            SafetySystems,
            [Display(Name = "Defect_ExhaustSystem", ResourceType = typeof(Text))]
            ExhaustSystem
        }
    }

    public class Payment
    {
        public decimal Price { get; set; }
        public PaymentType? PaymentMethod { get; set; }
        public bool PaymentAtContractFormation { get; set; }
        public DateOnly? PaymentDate { get; set; }
        public bool TransferInsurance { get; set; }
        public string? AdditionalInformation { get; set; }

        public enum PaymentType
        {
            [Display(Name = "PaymentType_Cash", ResourceType = typeof(Text))]
            Cash,
            [Display(Name = "PaymentType_BankTransfer", ResourceType = typeof(Text))]
            BankTransfer
        }
    }
}