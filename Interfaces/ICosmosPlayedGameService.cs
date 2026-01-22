using cosmosdb_test.Models;

namespace cosmosdb_test.Interfaces;

public interface ICosmosPlayedGameService
{
    Task<PlayedGame?> AddAsync(PlayedGame game);
    Task<IEnumerable<PlayedGame>> GetAllAsync();
}
