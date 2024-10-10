using FastEndpoints;

namespace Api.Contracts;

public static class Mappings
{
    public static Contract ToEntity(ContractRequest req) => new()
    {
        Buyer = req.Buyer is null ? null : ToEntity(req.Buyer),
        Seller = req.Seller is null ? null : ToEntity(req.Seller),
        Vehicle = req.Vehicle is null ? null : ToEntity(req.Vehicle)
    };

    public static ContractResponse FromEntity(Contract entity) => new()
    {
        Id = entity.Id,
        Buyer = entity.Buyer is null ? null : FromEntity(entity.Buyer),
        Seller = entity.Seller is null ? null : FromEntity(entity.Seller),
        Vehicle = entity.Vehicle is null ? null : FromEntity(entity.Vehicle)
    };

    private static Contract.PartyInfo ToEntity(PartyInfo req) => new()
    {
        Name = req.Name,
        Address = req.Address,
        Company = req.Company,
        Code = req.Code,
        Email = req.Email,
        PhoneNumber = req.PhoneNumber
    };

    private static Contract.VehicleInfo ToEntity(VehicleInfo req) => new()
    {
        SDK = req.SDK,
        Make = req.Make,
        Model = req.Model,
        StateRegistrationNumber = req.StateRegistrationNumber,
        Mileage = req.Mileage,
        VIN = req.VIN,
        RegistrationCertificateNumber = req.RegistrationCertificateNumber,
        TechnicalInspectionValid = req.TechnicalInspectionValid,
        HadOwnershipIncidents = req.HadOwnershipIncidents,
        HadIncidentsBeforeOwnership = req.HadIncidentsBeforeOwnership,
        Defects = req.Defects.Select(d => (Contract.Defect)d).ToArray(),
        IncidentInformation = req.IncidentInformation
    };

    private static PartyInfo FromEntity(Contract.PartyInfo entity) => new()
    {
        Name = entity.Name,
        Address = entity.Address,
        Company = entity.Company,
        Code = entity.Code,
        Email = entity.Email,
        PhoneNumber = entity.PhoneNumber
    };

    private static VehicleInfo FromEntity(Contract.VehicleInfo entity) => new()
    {
        SDK = entity.SDK,
        Make = entity.Make,
        Model = entity.Model,
        StateRegistrationNumber = entity.StateRegistrationNumber,
        Mileage = entity.Mileage,
        VIN = entity.VIN,
        RegistrationCertificateNumber = entity.RegistrationCertificateNumber,
        TechnicalInspectionValid = entity.TechnicalInspectionValid,
        HadOwnershipIncidents = entity.HadOwnershipIncidents,
        HadIncidentsBeforeOwnership = entity.HadIncidentsBeforeOwnership,
        Defects = entity.Defects.Select(d => (Defect)d).ToArray(),
        IncidentInformation = entity.IncidentInformation
    };
}

public class ContractResponseMapper : ResponseMapper<ContractResponse, Contract>
{
    public override ContractResponse FromEntity(Contract e) => Mappings.FromEntity(e);
}

public class ContractMapper : Mapper<ContractRequest, ContractResponse, Contract>
{
    public override Contract ToEntity(ContractRequest req) => Mappings.ToEntity(req);
    public override ContractResponse FromEntity(Contract entity) => Mappings.FromEntity(entity);
}
