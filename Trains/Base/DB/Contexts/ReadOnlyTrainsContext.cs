using Microsoft.EntityFrameworkCore;

namespace Trains.Base.DB.Contexts;

public sealed class ReadOnlyTrainsContext(DbContextOptions<ReadOnlyTrainsContext> options) : DbContext(options), IEntityDefinitions
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configure your entities here
    }
    // Define DbSets for your entities
    // public DbSet<YourEntity> YourEntities { get; set; }
}
