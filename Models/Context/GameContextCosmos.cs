using Microsoft.EntityFrameworkCore;

namespace cosmosdb_test.Models.Context;

public class GameContextCosmos : DbContext
{
    public GameContextCosmos(DbContextOptions<GameContextCosmos> options) : base(options)
    {
    }

    public DbSet<PlayedGame> PlayedGames { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlayedGame>(entity =>
        {
            entity.ToContainer("PlayedGame").HasPartitionKey(pg => pg.Id);
            entity.Property(pg => pg.Id).HasConversion<string>().ToJsonProperty("id");
            entity.HasNoDiscriminator();

            entity.Property(pg => pg.PlayerMove).HasConversion<string>();
            entity.Property(pg => pg.MachineMove).HasConversion<string>();
            entity.Property(pg => pg.MatchResult).HasConversion<string>();
        });
    }
}
