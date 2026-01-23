using Microsoft.EntityFrameworkCore;
using cosmosdb_test.Repositories.Interfaces;
using cosmosdb_test.Repositories.Models;
using cosmosdb_test.Repositories.Context;

namespace cosmosdb_test.Repositories;

public class SqlRepository : ISqlRepository
{
    private readonly ILogger<SqlRepository> _logger;
    private readonly SqlContext _context;
    
    public SqlRepository(ILogger<SqlRepository> logger, SqlContext context)
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
