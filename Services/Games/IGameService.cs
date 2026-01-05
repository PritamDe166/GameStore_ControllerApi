namespace GameStoreControllerApi.Services.Games;

public interface IGameService
{
    Task<List<GetGamesDto>> GetAllGamesAsync();
    Task<GetGamesDto?> GetGameByIdAsync(int id);
    Task<List<GetGamesDto>> GetGamesByNameAsync(string name);
    Task<GetGamesDto> CreateGameAsync(CreateGameDto newGame);
    Task<GetGamesDto> UpdateGameAsync(int id, CreateGameDto updatedGame);
    Task<bool> DeleteGameAsync(int id);
}
