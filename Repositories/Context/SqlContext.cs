using cosmosdb_test.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace cosmosdb_test.Repositories.Context;


public class SqlContext : DbContext
{
    public SqlContext(DbContextOptions<SqlContext> options) : base(options)
    {
    }

    public DbSet<CompletedGameState> CompletedGameStates { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CompletedGameState>(entity =>
        {
            entity.ToTable("CompletedGameStates");
            
            entity.HasKey(pg => pg.Id);

            entity.Property(pg => pg.Id).ValueGeneratedNever();   // eller ValueGeneratedOnAdd

            entity.Property(pg => pg.PlayerMove);
            entity.Property(pg => pg.MachineMove);
            entity.Property(pg => pg.MatchResult);
        });
    }
}
