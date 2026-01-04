using GameStoreControllerApi.Dto.Game;
using GameStoreControllerApi.Mappings;

namespace GameStoreControllerApi.Endpoints;

public static class GameEndpoints
{
    public static RouteGroupBuilder MapGameEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games");

        group.MapGet("/", async (GameStoreContext dbContext) =>
        {
            return await dbContext.games
                                  .Include(g => g.Genre)
                                  .Select(g => g.EntityToDto())
                                  .ToListAsync();
        });

        group.MapPost("/", async (GameStoreContext dbContext, CreateGameDto newGame) =>
        {
            Game game = newGame.DtoToEntity();
            game.Genre = await dbContext.genres.FindAsync(newGame.GenreId);

            dbContext.games.Add(game);
            await dbContext.SaveChangesAsync();


            return Results.Created($"games/{game.Id}", game.EntityToDto());
        });

        return group;
    }
}
