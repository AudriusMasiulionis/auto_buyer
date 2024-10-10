using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Api.Contracts.Commands;
using FastEndpoints;

namespace Api.Contracts.Endpoints;

public class ContractPost(IAmazonDynamoDB dynamoDbClient) : Endpoint<ContractRequest, ContractResponse, ContractMapper>
{
    private readonly DynamoDBContext _context = new(dynamoDbClient);

    public override void Configure()
    {
        Post("/api/contracts");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ContractRequest req, CancellationToken ct)
    {
        Contract entity = Map.ToEntity(req);
        await _context.SaveAsync(entity, ct);
        ContractResponse response = Map.FromEntity(entity);
        await new SellerSigningCompleted(entity.Id).QueueJobAsync(ct: ct);
        await SendCreatedAtAsync<ContractGet>($"/api/contract/{entity.Id}", response, cancellation: ct);
    }
}

