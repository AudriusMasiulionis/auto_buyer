using Amazon.DynamoDBv2;
using Api.Tables;
using FastEndpoints;

namespace Api.Endpoints;

public class ContractPostEndpoint(IAmazonDynamoDB dynamoDbClient) : Endpoint<Contract, Contract>
{
    private readonly IAmazonDynamoDB _dynamoDbClient = dynamoDbClient;

    public override void Configure()
    {
        Post("/api/contracts");
        AllowAnonymous();
    }

    public override async Task HandleAsync(Contract req, CancellationToken ct)
    {
        await SendAsync(req, cancellation: ct);
    }
}

