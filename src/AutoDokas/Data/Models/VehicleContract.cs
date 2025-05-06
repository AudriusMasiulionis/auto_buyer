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

    public class PartyInfo
    {
        public string? Code { get; set; }
        public bool IsCompany { get; set; }
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
        public int Millage { get; set; }
        public string IdentificationNumber { get; set; }
        public bool IsInspected { get; set; }
        public bool HasBeenDamaged { get; set; }
        public bool PriorDamagesKnown { get; set; }
        public List<Defect> Defects { get; set; } = [];
        public string? AdditionalInformation { get; set; }

        public enum Defect
        {
            Brakes,
            Engine,
            Transmission,
            Suspension,
            Steering,
            ElectricalSystem,
            Bodywork,
            Interior,
            PassengerSafetySystems,
            Lights,
            ExhaustSystem,
            Other
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
            Cash,
            BankTransfer
        }
    }
}