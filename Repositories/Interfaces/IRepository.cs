using cosmosdb_test.Repositories.Models;

namespace cosmosdb_test.Repositories.Interfaces;

public interface IRepository
{
    Task<CompletedGameState?> AddAsync(CompletedGameState game);
    Task<IEnumerable<CompletedGameState>> GetAllAsync();
}
