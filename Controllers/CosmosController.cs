using cosmosdb_test.Interfaces;
using cosmosdb_test.Dto;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using cosmosdb_test.Models;

namespace cosmosdb_test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CosmosController : ControllerBase
{
    private readonly ILogger<CosmosController> _logger;
    private readonly IMapper _mapper;
    private readonly IGameState _gameState;
    private readonly ICosmosPlayedGameService _playedGameService;

    public CosmosController(ILogger<CosmosController> logger, IMapper mapper, IGameState gamestate, ICosmosPlayedGameService playedGameService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _gameState = gamestate ?? throw new ArgumentNullException(nameof(gamestate));
        _playedGameService = playedGameService ?? throw new ArgumentNullException(nameof(playedGameService));
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

            PlayedGame? playedGame = await _playedGameService.AddAsync(_mapper.Map<PlayedGame>(_gameState));
            if (playedGame == null)
            {
                _logger.LogError("could not store game to DB");
                return StatusCode(500, "Kunne ikke large til database");
            }

            return new JsonResult(_mapper.Map<GameStateDto>(playedGame));
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
            IEnumerable<PlayedGame> games = await _playedGameService.GetAllAsync();
            GameStatisticsDto statistics = new GameStatisticsDto(games);
            
            return new JsonResult(statistics);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error: {ex.Message}");
            return new ObjectResult("En feil oppstod!"){StatusCode = 500};
        }
    }
}
