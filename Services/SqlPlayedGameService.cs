using cosmosdb_test.Interfaces;
using cosmosdb_test.Models;
using cosmosdb_test.Models.Context;
using Microsoft.EntityFrameworkCore;

namespace cosmosdb_test.Services;

public class SqlPlayedGameService : ISqlPlayedGameService
{
    private readonly ILogger<CosmosPlayedGameService> _logger;
    private readonly GameContextSql _context;
    
    public SqlPlayedGameService(ILogger<CosmosPlayedGameService> logger, GameContextSql context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<PlayedGame?> AddAsync(PlayedGame game)
    {
        try
        {
            _context.PlayedGames.Add(game);
            await _context.SaveChangesAsync();
            return game;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.ToString());
            return null;
        }
    }

    public async Task<IEnumerable<PlayedGame>> GetAllAsync()
    {
        return await _context.PlayedGames.ToListAsync();
    }

}
