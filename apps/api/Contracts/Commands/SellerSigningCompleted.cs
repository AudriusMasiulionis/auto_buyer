using FastEndpoints;

namespace Api.Contracts.Commands;

public record SellerSigningCompleted(Guid ContractId) : ICommand;

public class SellerSigningCompletedHandler : ICommandHandler<SellerSigningCompleted>
{
    public Task ExecuteAsync(SellerSigningCompleted command, CancellationToken ct)
    {
        return Task.Delay(5 * 1000, ct);
    }
}