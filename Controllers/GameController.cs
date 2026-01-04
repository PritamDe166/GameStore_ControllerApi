
namespace GameStoreControllerApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameController : ControllerBase
{
    public readonly GameStoreContext _dbContext;

    public GameController(GameStoreContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<List<GetGamesDto>>> GetGames()
    {
        var games = await _dbContext.games
                                    .Include(g => g.Genre)
                                    .Select(g => g.EntityToDto())
                                    .ToListAsync();
        return Ok(games);
    }

    [HttpGet("{id:int}" , Name = "GetById")]
    public async Task<ActionResult<GetGamesDto>> GetGameById(int id)
    {
        var game = await _dbContext.games
                                   .Include(g => g.Genre)
                                   .Where(g => g.Id == id)
                                   .Select(g => g.EntityToDto())
                                   .FirstOrDefaultAsync();
        if (game == null)
        {
            return NotFound();
        }
        return Ok(game);
    }

    [HttpGet("_searchByName")]
    public async Task<ActionResult<List<GetGamesDto>>> GetGamesbyName([FromQuery] string name)
    {
        var games = await _dbContext.games
                                    .Include(g => g.Genre)
                                    .Where(g => g.Name == name)
                                    .Select(g => g.EntityToDto())
                                    .ToListAsync();

        if(games.Contains(null) || games.Count == 0)
        {
            return NotFound();
        }

        return Ok(games);
    }

    [HttpPost]
    public async Task<ActionResult<GetGamesDto>> CreateNewGame([FromBody] CreateGameDto newGame, [FromServices] IValidator<CreateGameDto> validator)
    {
        await validator.ValidateOrThrowAsync(newGame); //Validation

        Game game = newGame.DtoToEntity();
        game.Genre = await _dbContext.genres.FindAsync(newGame.GenreId);
        _dbContext.games.Add(game);
        await _dbContext.SaveChangesAsync();
        return CreatedAtRoute("GetById", new { id = game.Id }, game.EntityToDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteGame(int id)
    {
        var game = await _dbContext.games.FindAsync(id);
        if (game == null)
        {
            return NotFound();
        }
        _dbContext.games.Remove(game);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}
