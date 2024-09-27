using Amazon.DynamoDBv2.DataModel;

namespace Api.Models;

[DynamoDBTable("Contracts")]
public class Contract
{
    public Guid Id { get; set; }
    public PartyInfo? Buyer { get; set; } // todo encrypt
    // public PartyInfo Seller { get; set; } // todo encrypt
    // public VehicleInfo Vehicle { get; set; } // todo

    // public Status Status { get; set; } todo

    // todo status logic
}