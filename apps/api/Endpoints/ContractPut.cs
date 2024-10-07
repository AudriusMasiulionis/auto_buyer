using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Api.Tables;
using FastEndpoints;

namespace Api.Endpoints;

public class ContractPut(IAmazonDynamoDB dynamoDbClient) : Endpoint<Contract, Contract>
{
    private readonly DynamoDBContext _context = new(dynamoDbClient);

    public override void Configure()
    {
        Put("/api/contracts/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Contract req, CancellationToken ct)
    {
        var id = Route<string>("id");
        var existing = await _context.LoadAsync<Contract>(id, ct);
        if (existing == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        existing.Buyer = req.Buyer;
        existing.Seller = req.Seller;
        existing.Vehicle = req.Vehicle;

        await _context.SaveAsync(existing, ct);
        await SendAsync(existing, cancellation: ct);
    }
}

