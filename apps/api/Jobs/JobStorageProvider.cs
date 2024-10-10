using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using FastEndpoints;

namespace Api.Jobs;

public sealed class JobStorageProvider(IDynamoDBContext context) : IJobStorageProvider<JobRecord>
{
    private readonly IDynamoDBContext _context = context;

    public Task StoreJobAsync(JobRecord job, CancellationToken ct)
    {
        try
        {
            return _context.SaveAsync(job, ct);
        }
        catch (System.Exception e)
        {
            throw;            
        }
    }

    public async Task<IEnumerable<JobRecord>> GetNextBatchAsync(PendingJobSearchParams<JobRecord> p)
    {
        var conditions = new List<ScanCondition>
        {
            new(nameof(JobRecord.QueueID), ScanOperator.Equal, p.QueueID),
            new(nameof(JobRecord.IsComplete), ScanOperator.Equal, false),
            new(nameof(JobRecord.ExecuteAfter), ScanOperator.LessThanOrEqual, DateTime.UtcNow),
            new(nameof(JobRecord.ExpireOn), ScanOperator.GreaterThanOrEqual, DateTime.UtcNow)
        };

        var search = _context.ScanAsync<JobRecord>(conditions);
        var batch = await search.GetNextSetAsync(p.CancellationToken);
        return batch.Take(p.Limit);
    }

    public async Task MarkJobAsCompleteAsync(JobRecord job, CancellationToken ct)
    {
        var existingJob = await _context.LoadAsync<JobRecord>(job.Id, ct);
        if (existingJob is null) return;

        existingJob.IsComplete = true;
        await _context.SaveAsync(existingJob, ct);
    }

    public async Task CancelJobAsync(Guid trackingId, CancellationToken ct)
    {
        var conditions = new List<ScanCondition>
        {
            new(nameof(JobRecord.TrackingID), ScanOperator.Equal, trackingId)
        };

        var search = _context.ScanAsync<JobRecord>(conditions);
        var jobs = await search.GetRemainingAsync(ct);
        var tasks = jobs.Select(job =>
        {
            job.IsCancelled = true;
            return _context.SaveAsync(job, ct);
        });
        await Task.WhenAll(tasks);
    }

    public async Task OnHandlerExecutionFailureAsync(JobRecord job, Exception e, CancellationToken c)
    {
        var existingJob = await _context.LoadAsync<JobRecord>(job.Id, c);
        if (existingJob is null) return;

        existingJob.ExecuteAfter = DateTime.UtcNow.AddMinutes(1);
        await _context.SaveAsync(existingJob, c);
    }

    public async Task PurgeStaleJobsAsync(StaleJobSearchParams<JobRecord> p)
    {
        var conditions = new List<ScanCondition>
        {
            new(nameof(JobRecord.IsComplete), ScanOperator.Equal, true),
            new(nameof(JobRecord.ExpireOn), ScanOperator.LessThanOrEqual, DateTime.UtcNow)
        };

        var search = _context.ScanAsync<JobRecord>(conditions);
        var jobs = await search.GetRemainingAsync(p.CancellationToken);

        var tasks = jobs.Select(job => _context.DeleteAsync(job, p.CancellationToken));
        await Task.WhenAll(tasks);
    }
}