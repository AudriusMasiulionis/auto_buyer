using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Api.Tables;
using FastEndpoints;

namespace Api.Endpoints;

public class ContractPostEndpoint(IAmazonDynamoDB dynamoDbClient) : Endpoint<Contract, Contract>
{
    private readonly DynamoDBContext _context = new(dynamoDbClient);


    public override void Configure()
    {
        Post("/api/contracts");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Contract req, CancellationToken ct)
    {
        Contract entity = new()
        {
            Id = Guid.NewGuid().ToString(),
        };

        await _context.SaveAsync(entity, ct);
        await SendAsync(entity, cancellation: ct);
    }
}

