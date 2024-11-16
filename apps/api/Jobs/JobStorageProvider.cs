using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Api.Jobs;

sealed class JobStorageProvider : IJobStorageProvider<JobRecord>, IJobResultProvider
{
    private readonly PooledDbContextFactory<JobDbContext> _dbPool;

    public JobStorageProvider()
    {
        var dbFolderPath = Path.Combine(Environment.CurrentDirectory, "Data");
        if (Directory.Exists(dbFolderPath) == false)
            Directory.CreateDirectory(dbFolderPath);
        var dbPath = Path.Combine(dbFolderPath, "JobDatabase.db");
        var opts = new DbContextOptionsBuilder<JobDbContext>()
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            .UseSqlite($"Data Source={dbPath}").Options;
        _dbPool = new PooledDbContextFactory<JobDbContext>(opts);
        using var db = _dbPool.CreateDbContext();
        db.Database.Migrate();
    }

    public async Task StoreJobAsync(JobRecord job, CancellationToken ct)
    {
        await using var db = await _dbPool.CreateDbContextAsync(ct);
        await db.AddAsync(job, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<JobRecord>> GetNextBatchAsync(PendingJobSearchParams<JobRecord> p)
    {
        await using var db = await _dbPool.CreateDbContextAsync();
        return await db.Jobs
            .Where(p.Match)
            .Take(p.Limit)
            .ToListAsync(p.CancellationToken);
    }

    public async Task MarkJobAsCompleteAsync(JobRecord job, CancellationToken c)
    {
        await using var db = await _dbPool.CreateDbContextAsync(c);
        db.Update(job);
        await db.SaveChangesAsync(c);
    }

    public async Task CancelJobAsync(Guid trackingId, CancellationToken c)
    {
        await using var db = await _dbPool.CreateDbContextAsync(c);
        var job = await db.Jobs.FirstOrDefaultAsync(j => j.TrackingID == trackingId, cancellationToken: c);

        if (job is not null)
        {
            job.IsComplete = true;
            db.Update(job);
            await db.SaveChangesAsync(c);
        }
    }

    public async Task OnHandlerExecutionFailureAsync(JobRecord job, Exception e, CancellationToken c)
    {
        await using var db = await _dbPool.CreateDbContextAsync(c);
        job.ExecuteAfter = DateTime.UtcNow.AddMinutes(1);
        db.Update(job);
        await db.SaveChangesAsync(c);
    }

    public async Task PurgeStaleJobsAsync(StaleJobSearchParams<JobRecord> p)
    {
        await using var db = await _dbPool.CreateDbContextAsync();
        var staleJobs = db.Jobs.Where(p.Match);
        db.RemoveRange(staleJobs);
        await db.SaveChangesAsync(p.CancellationToken);
    }

    public async Task StoreJobResultAsync<TResult>(Guid trackingId, TResult result, CancellationToken c)
    {
        await using var db = await _dbPool.CreateDbContextAsync(c);
        var job = await db.Jobs.SingleAsync(j => j.TrackingID == trackingId, cancellationToken: c);

        ((IJobResultStorage)job).SetResult(result);
        db.Update(job);
        await db.SaveChangesAsync(c);
    }

    public async Task<TResult?> GetJobResultAsync<TResult>(Guid trackingId, CancellationToken c)
    {
        await using var db = await _dbPool.CreateDbContextAsync(c);
        var job = await db.Jobs.FirstOrDefaultAsync(j => j.TrackingID == trackingId, cancellationToken: c);

        return job is not null
            ? ((IJobResultStorage)job).GetResult<TResult>()
            : default;
    }
}