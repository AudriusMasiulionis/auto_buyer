using FastEndpoints;

namespace Api.Contracts.Endpoints;

public class ContractGet(ApplicationDbContext context)
    : EndpointWithoutRequest<ContractResponse, ContractResponseMapper>
{
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
        var id = Route<Guid>("id");
        var contract = await context.FindAsync<Contract>(keyValues: [id], cancellationToken: ct);
        if (contract == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var response = Map.FromEntity(contract);
        await SendAsync(response, cancellation: ct);
    }
}