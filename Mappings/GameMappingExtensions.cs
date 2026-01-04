using GameStoreControllerApi.Dto.Game;

namespace GameStoreControllerApi.Mappings;

public static class GameMappingExtensions
{
    public static GetGamesDto EntityToDto(this Game game)
    {
        return new GetGamesDto(
            game.Id,
            game.Name,
            game.GenreId,
            game.Genre?.Name ?? string.Empty,
            game.Price,
            game.ReleaseDate
        );
    }


    public static Game DtoToEntity(this CreateGameDto createGameDto)
    {
        return new Game()
        {
            Name = createGameDto.Name,
            GenreId = createGameDto.GenreId,
            Price = createGameDto.Price,
            ReleaseDate = createGameDto.ReleaseDate
        };
    }
}
