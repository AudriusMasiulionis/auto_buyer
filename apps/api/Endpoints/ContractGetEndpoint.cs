using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Api.Tables;
using FastEndpoints;

namespace Api.Endpoints;

public class ContractGetEndpoint(IAmazonDynamoDB dynamoDbClient) : EndpointWithoutRequest<Contract>
{
    private readonly DynamoDBContext _context = new(dynamoDbClient);

    public override void Configure()
    {
        Get("/api/contracts/{id}");
        Description(b => b
            .Produces<Contract>()
            .WithName("GetContract")
            .WithSummary("Get a contract by ID"));
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<string>("id");
        var contract = await _context.LoadAsync<Contract>(id, ct);
        if (contract == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        
        await SendAsync(contract, cancellation: ct);
    }
}

