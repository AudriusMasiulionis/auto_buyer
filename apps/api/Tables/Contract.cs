using Amazon.DynamoDBv2.DataModel;

namespace Api.Tables;

[DynamoDBTable("Contracts")]
public class Contract
{
    public Guid Id { get; set; }
    public PartyInfo? Buyer { get; set; } // todo encrypt
    // public PartyInfo Seller { get; set; } // todo encrypt
    // public VehicleInfo Vehicle { get; set; } // todo

    // public Status Status { get; set; } todo

    // todo status logic

    public class PartyInfo
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Code { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    // add prop to store signature byte[]
}

public enum Status
{
    Draft,
    Active,
    Completed
}

public class VehicleInfo
{
    public string SDK { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    // public int MyProperty { get; set; } valstybinis registracijos numeris
    public int Mileage { get; set; }
    public string VIN { get; set; }
    // public int MyProperty { get; set; } Transporto priemones registracijos liudijimo serija ir numeris
    public bool TechnicalInspectionValid { get; set; }
    public bool HadOwnershipIncidents { get; set; }
    public bool HadIncidentsBeforeOwnership { get; set; }
    public string[] Defects { get; set; }
    public string IncidentInformation { get; set; }
}
}