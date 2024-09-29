using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Api.Models;
using FastEndpoints;

namespace Api.Endpoints;

public class GetContractRequest
{
    public string Id { get; set; }
}

public class ContractGetEndpoint : Endpoint<GetContractRequest, Contract>
{
    private readonly DynamoDBContext _context;

    public ContractGetEndpoint(IAmazonDynamoDB dynamoDbClient)
    {
        _context = new DynamoDBContext(dynamoDbClient);    
    }

    public override void Configure()
    {
        Get("/api/user/{id}");
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

