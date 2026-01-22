using Microsoft.EntityFrameworkCore;

namespace cosmosdb_test.Models.Context;


public class GameContextSql : DbContext
{
    public GameContextSql(DbContextOptions<GameContextSql> options) : base(options)
    {
    }

    public DbSet<PlayedGame> PlayedGames { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlayedGame>(entity =>
        {
            entity.ToTable("PlayedGame");
            
            entity.HasKey(pg => pg.Id);

            entity.Property(pg => pg.Id).ValueGeneratedNever();   // eller ValueGeneratedOnAdd

            entity.Property(pg => pg.PlayerMove);
            entity.Property(pg => pg.MachineMove);
            entity.Property(pg => pg.MatchResult);
        });
    }
}
