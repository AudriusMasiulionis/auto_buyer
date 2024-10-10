
namespace Api.Contracts;

public class ContractRequest
{
    public PartyInfo? Buyer { get; set; }
    public PartyInfo? Seller { get; set; }
    public VehicleInfo? Vehicle { get; set; }
}

public class ContractResponse
{
    public required Guid Id { get; set; }
    public PartyInfo? Buyer { get; set; }
    public PartyInfo? Seller { get; set; }
    public VehicleInfo? Vehicle { get; set; }
}


    public class PartyInfo
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public bool Company { get; set; } = false;
        public required string Code { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
}

    public class VehicleInfo
    {
        public string SDK { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string StateRegistrationNumber { get; set; }
        public int Mileage { get; set; }
        public string VIN { get; set; }
        public string RegistrationCertificateNumber { get; set; }
        public bool TechnicalInspectionValid { get; set; }
        public bool HadOwnershipIncidents { get; set; }
        public bool HadIncidentsBeforeOwnership { get; set; }
        public Defect[] Defects { get; set; } = [];
        public string IncidentInformation { get; set; }
    }

    public enum Defect
    {
        Brakes,
        Lighting,
        Exhaust,
        SafetySystems,
        SteeringOrSuspension,
        Other
    }