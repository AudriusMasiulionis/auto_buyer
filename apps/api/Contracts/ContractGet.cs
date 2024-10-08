using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using FastEndpoints;

namespace Api.Contracts;

public class ContractGet(IAmazonDynamoDB dynamoDbClient) : EndpointWithoutRequest<ContractResponse, ContractResponseMapper>
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

        ContractResponse response = Map.FromEntity(contract);
        await SendAsync(response, cancellation: ct);
    }
}

