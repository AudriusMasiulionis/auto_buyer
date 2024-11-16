using Microsoft.EntityFrameworkCore;

namespace Api.Jobs;

public class JobDbContext(DbContextOptions<JobDbContext> options) : DbContext(options)
{
    public DbSet<JobRecord> Jobs { get; set; }
}