using FastEndpoints;

namespace Api.Contracts.Commands;

public record BuyerSigningCompleted(string ContractId) : ICommand;

public class BuyerSigningCompletedHandler : ICommandHandler<BuyerSigningCompleted>
{
    public Task ExecuteAsync(BuyerSigningCompleted command, CancellationToken ct)
    {
        return Task.Delay(5 * 1000, ct);
    }
}
