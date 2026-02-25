using AutoDokas.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoDokas.Services;

public class DataRetentionService(
    IDbContextFactory<AppDbContext> dbContextFactory,
    ILogger<DataRetentionService> logger) : BackgroundService
{
    private const int RetentionDays = 30;
    private const int BatchSize = 100;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Data retention service started (retention: {Days} days, batch size: {BatchSize})", RetentionDays, BatchSize);

        using var timer = new PeriodicTimer(TimeSpan.FromHours(24));

        // Run immediately on startup, then every 24 hours
        do
        {
            logger.LogInformation("Data retention cleanup run starting");
            try
            {
                await AnonymizeExpiredContractsAsync(stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogError(ex, "Error during data retention cleanup");
            }
        } while (await timer.WaitForNextTickAsync(stoppingToken));

        logger.LogInformation("Data retention service stopped");
    }

    private async Task AnonymizeExpiredContractsAsync(CancellationToken stoppingToken)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-RetentionDays);
        logger.LogInformation("Looking for contracts created before {CutoffDate:yyyy-MM-dd HH:mm:ss} UTC", cutoffDate);

        var totalAnonymized = 0;
        var batchNumber = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            await using var db = await dbContextFactory.CreateDbContextAsync(stoppingToken);

            var contracts = await db.VehicleContracts
                .IgnoreQueryFilters()
                .Where(c => c.CreatedAt < cutoffDate && c.AnonymizedAt == null)
                .Take(BatchSize)
                .ToListAsync(stoppingToken);

            if (contracts.Count == 0)
                break;

            batchNumber++;
            logger.LogInformation("Processing batch {Batch}: {Count} contracts to anonymize", batchNumber, contracts.Count);

            foreach (var contract in contracts)
            {
                logger.LogDebug("Anonymizing contract {ContractId} (created {CreatedAt:yyyy-MM-dd})", contract.Id, contract.CreatedAt);

                contract.SellerInfo = null;
                contract.BuyerInfo = null;

                if (contract.VehicleInfo is not null)
                {
                    contract.VehicleInfo.RegistrationNumber = null!;
                    contract.VehicleInfo.Sdk = null!;
                    contract.VehicleInfo.RegistrationCertificate = null;
                    contract.VehicleInfo.AdditionalInformation = null;
                }

                if (contract.PaymentInfo is not null)
                {
                    contract.PaymentInfo.AdditionalInformation = null;
                }

                contract.AnonymizedAt = DateTime.UtcNow;
            }

            await db.SaveChangesAsync(stoppingToken);
            totalAnonymized += contracts.Count;
        }

        logger.LogInformation("Data retention cleanup finished: {Count} contracts anonymized", totalAnonymized);
    }
}
