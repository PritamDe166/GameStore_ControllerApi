
using GameStoreControllerApi.Dto.Contracts.Pagination;

namespace GameStoreControllerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameController : ControllerBase
{
    public readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet]
    public async Task<ActionResult<PaginationResponse<GetGamesDto>>> GetGamesPaged([FromQuery] PaginationRequest paginationRequest)
    {
        var pagedGames = await _gameService.GetPagedGamesAsync(paginationRequest);
        return Ok(pagedGames);
    }

    [HttpGet("{id:int}" , Name = "GetById")]
    public async Task<ActionResult<GetGamesDto>> GetGameById(int id)
    {
        var game = await _gameService.GetGameByIdAsync(id);

        if (game == null)
        {
            return NotFound();
        }

        return Ok(game);
    }

    [HttpGet("_searchByName")]
    public async Task<ActionResult<List<GetGamesDto>>> GetGamesbyName([FromQuery] string name)
    {
        var games = await _gameService.GetGamesByNameAsync(name);

        if (games.Count == 0)
        {
            return NotFound();
        }

        return Ok(games);
    }

    [HttpPost]
    public async Task<ActionResult<GetGamesDto>> CreateNewGame([FromBody] CreateGameDto newGame)
    {
        var createdGame = await _gameService.CreateGameAsync(newGame);
        return CreatedAtRoute("GetById", new { id = createdGame.Id}, createdGame);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateGame([FromRoute] int id, [FromBody] UpdateGameDto updatedGame)
    {
        await _gameService.UpdateGameAsync(id, updatedGame);
        
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteGame(int id)
    {
        await _gameService.DeleteGameAsync(id);
        
        return NoContent();
    }
}
