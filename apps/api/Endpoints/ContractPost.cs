using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Api.Tables;
using FastEndpoints;

namespace Api.Endpoints;

public class ContractPost(IAmazonDynamoDB dynamoDbClient) : Endpoint<Contract, Contract>
{
    private readonly DynamoDBContext _context = new(dynamoDbClient);

    public override void Configure()
    {
        Post("/api/contracts");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Contract req, CancellationToken ct) // todo to different object
    {
        Contract entity = new()
        {
            Id = Guid.NewGuid().ToString(),
            Buyer = req.Buyer,
            Seller = req.Seller,
            Vehicle = req.Vehicle
        };

        await _context.SaveAsync(entity, ct);
        await SendAsync(entity, cancellation: ct);
    }
}

