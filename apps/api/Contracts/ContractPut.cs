using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using FastEndpoints;

namespace Api.Contracts;

public partial class ContractPut(IAmazonDynamoDB dynamoDbClient) : Endpoint<ContractRequest, ContractResponse, ContractMapper>
{
    private readonly DynamoDBContext _context = new(dynamoDbClient);

    public override void Configure()
    {
        Put("/api/contracts/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ContractRequest req, CancellationToken ct)
    {
        var id = Route<string>("id");
        var existing = await _context.LoadAsync<Contract>(id, ct);
        if (existing == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var updated = Map.ToEntity(req);

        existing.Buyer = updated.Buyer;
        existing.Seller = updated.Seller;
        existing.Vehicle = updated.Vehicle;

        await _context.SaveAsync(existing, ct);
        var response = Map.FromEntity(existing);
        await SendAsync(response, cancellation: ct);
    }
}

