using Amazon.DynamoDBv2.DataModel;
using Api.Helpers;

namespace Api.Contracts;

[DynamoDBTable("Contracts")]
public class Contract
{
    [DynamoDBProperty(Converter = typeof(GuidConverter))]
    [DynamoDBHashKey]
    public Guid Id { get; init; } = Guid.NewGuid();
    public PartyInfo? Buyer { get; set; } // todo encrypt
    public PartyInfo? Seller { get; set; } // todo encrypt
    public VehicleInfo? Vehicle { get; set; }

    public class PartyInfo
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public bool Company { get; set; } = false;
        public required string Code { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }

        // add prop to store signature byte[]?

        public override bool Equals(object? obj)
        {
            return obj is PartyInfo info &&
                   Name == info.Name &&
                   Address == info.Address &&
                   Company == info.Company &&
                   Code == info.Code &&
                   Email == info.Email &&
                   PhoneNumber == info.PhoneNumber;
        }

    }

    public enum Status
    {
        Draft,
        Active,
        Completed
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
}