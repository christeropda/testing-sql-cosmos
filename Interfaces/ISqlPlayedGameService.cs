using cosmosdb_test.Models;

namespace cosmosdb_test.Interfaces;

public interface ISqlPlayedGameService
{
    Task<PlayedGame?> AddAsync(PlayedGame game);
    Task<IEnumerable<PlayedGame>> GetAllAsync();
}

