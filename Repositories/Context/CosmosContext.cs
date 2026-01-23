using cosmosdb_test.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace cosmosdb_test.Repositories.Context;

public class CosmosContext : DbContext
{
    public CosmosContext(DbContextOptions<CosmosContext> options) : base(options)
    {
    }

    public DbSet<CompletedGameState> CompletedGameStates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CompletedGameState>(entity =>
        {
            entity.ToContainer("CompletedGameStates").HasPartitionKey(pg => pg.Id);
            entity.Property(pg => pg.Id).HasConversion<string>().ToJsonProperty("id");
            entity.HasNoDiscriminator();

            entity.Property(pg => pg.PlayerMove).HasConversion<string>();
            entity.Property(pg => pg.MachineMove).HasConversion<string>();
            entity.Property(pg => pg.MatchResult).HasConversion<string>();
        });
    }
}
