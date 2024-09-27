namespace Api.Models;

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

