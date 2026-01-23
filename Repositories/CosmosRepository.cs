using cosmosdb_test.Repositories.Context;
using cosmosdb_test.Repositories.Interfaces;
using cosmosdb_test.Repositories.Models;
using Microsoft.EntityFrameworkCore;

namespace cosmosdb_test.Repositories;

public class CosmosRepository : ICosmosRepository
{
    private readonly ILogger<CosmosRepository> _logger;
    private readonly CosmosContext _context;
    public CosmosRepository(ILogger<CosmosRepository> logger, CosmosContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<CompletedGameState?> AddAsync(CompletedGameState completedGameState)
    {
        if (completedGameState.Id == Guid.Empty)
            completedGameState.Id = Guid.NewGuid();

        _context.CompletedGameStates.Add(completedGameState);
        await _context.SaveChangesAsync();

        return completedGameState;
    }

    public async Task<IEnumerable<CompletedGameState>> GetAllAsync()
    {
        return await _context.CompletedGameStates.ToListAsync();
    }
}
