using FastEndpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFastEndpoints();

var app = builder.Build();

app.UseFastEndpoints();

app.Run();



public class MyRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
}

public class MyResponse
{
    public string FullName { get; set; }
    public bool IsOver18 { get; set; }
}

public class Contract
{
    public PartyInfo Buyer { get; set; } // todo encrypt
    public PartyInfo Seller { get; set; } // todo encrypt
    public VehicleInfo Vehicle { get; set; }

    public Status Status { get; set; }

    // todo status logic
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

public enum Status
{
    Draft,
    Active,
    Completed
}

public class PartyInfo
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Code { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    // add prop to store signature byte[]
}

