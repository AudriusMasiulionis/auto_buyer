using Api.Contracts.Commands;
using FastEndpoints;

namespace Api.Contracts.Endpoints;

public class ContractPost(ApplicationDbContext context) : Endpoint<ContractRequest, ContractResponse, ContractMapper>
{
    public override void Configure()
    {
        Post("/api/contracts");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ContractRequest req, CancellationToken ct)
    {
        var entity = Map.ToEntity(req);
        context.Add(entity);
        await context.SaveChangesAsync(ct);
        var response = Map.FromEntity(entity);
        await new SellerSigningCompleted(entity.Id).QueueJobAsync(ct: ct);
        await SendCreatedAtAsync<ContractGet>($"/api/contract/{entity.Id}", response, cancellation: ct);
    }
}

