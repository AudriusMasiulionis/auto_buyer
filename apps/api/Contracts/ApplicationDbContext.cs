using Microsoft.EntityFrameworkCore;

namespace Api.Contracts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Contract> Contracts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contract>(entity =>
        {
            entity.OwnsOne(c => c.Buyer);
            entity.OwnsOne(c => c.Seller);
            entity.OwnsOne(c => c.Vehicle);
        });
    }
}