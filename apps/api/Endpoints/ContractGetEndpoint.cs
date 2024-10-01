using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Api.Tables;
using FastEndpoints;

namespace Api.Endpoints;

public class GetContractRequest
{
    public string Id { get; set; }
}

public class ContractGetEndpoint(IAmazonDynamoDB dynamoDbClient) : Endpoint<GetContractRequest, Contract>
{
    private readonly DynamoDBContext _context = new(dynamoDbClient);

    public override void Configure()
    {
        Get("/api/contracts/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetContractRequest req, CancellationToken ct)
    {
        var contract = await _context.LoadAsync<Contract>(req.Id, ct);
        if (contract == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        
        await SendAsync(contract, cancellation: ct);
    }
}

