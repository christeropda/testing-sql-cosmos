using cosmosdb_test.Interfaces;
using cosmosdb_test.Dto;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using cosmosdb_test.Repositories.Interfaces;
using cosmosdb_test.Repositories.Models;

namespace cosmosdb_test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CosmosController : ControllerBase
{
    private readonly ILogger<CosmosController> _logger;
    private readonly IMapper _mapper;
    private readonly IGameState _gameState;
    private readonly ICosmosRepository _repository;

    public CosmosController(ILogger<CosmosController> logger, IMapper mapper, IGameState gamestate, ICosmosRepository repository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _gameState = gamestate ?? throw new ArgumentNullException(nameof(gamestate));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet]
    [Route("game")]
    public async Task<IActionResult> GetGameResult([FromQuery] string? playerMove)
    {
        try
        {
            _logger.LogInformation("Request called");
            if (string.IsNullOrEmpty(playerMove))
            {
                _logger.LogInformation($"No user input: {nameof(playerMove)}");
                return new BadRequestObjectResult("Request må inneholde et spillertrekk");
            }

            _gameState.EvaluateWinner(playerMove);
            _logger.LogInformation($"Evaluated match: {_gameState.PlayerMove.ToString()} {_gameState.MatchResult.ToString()}");

            CompletedGameState? completedGame = await _repository.AddAsync(_mapper.Map<CompletedGameState>(_gameState));
            if (completedGame == null)
            {
                _logger.LogError("could not store game to DB");
                return StatusCode(500, "Kunne ikke large til database");
            }

            return new JsonResult(_mapper.Map<CompletedGameStateDto>(completedGame));
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError($"Error: {ex.Message}");
            return new BadRequestObjectResult("Request må inneholde et gyldig spillertrekk: stein/saks/papir");

        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}");
            return new ObjectResult("En feil oppstod!")
            {
                StatusCode = 500,
            };
        }
    }

    [HttpGet]
    [Route("Statistics")]
    public async Task<IActionResult> GetAllStatistics()
    {
        try
        {
            IEnumerable<CompletedGameState> completedGames = await _repository.GetAllAsync();
            CompletedGameStatisticsDto statistics = new CompletedGameStatisticsDto(completedGames);
            
            return new JsonResult(statistics);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}");
            return new ObjectResult("En feil oppstod!"){StatusCode = 500};
        }
    }
}
