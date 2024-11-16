using FastEndpoints;

namespace Api.Contracts.Endpoints;

public class ContractPut(ApplicationDbContext context) : Endpoint<ContractRequest, ContractResponse, ContractMapper>
{
    public override void Configure()
    {
        Put("/api/contracts/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ContractRequest req, CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var existing = await context.FindAsync<Contract>(keyValues: [id], cancellationToken: ct);
        if (existing == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var updated = Map.ToEntity(req);
        existing.Buyer = updated.Buyer;
        existing.Seller = updated.Seller;
        existing.Vehicle = updated.Vehicle;
        context.Update(existing);
        await context.SaveChangesAsync(ct);
        var response = Map.FromEntity(existing);
        await SendAsync(response, cancellation: ct);
    }
}