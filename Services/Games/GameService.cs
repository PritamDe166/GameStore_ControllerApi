
using GameStoreControllerApi.Dto.Contracts.Pagination;

namespace GameStoreControllerApi.Services.Games;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IValidationService _validationService;

    public GameService(IGameRepository gameRepository, IGenreRepository genreRepository, IValidationService validationService)
    {
        _gameRepository = gameRepository;
        _genreRepository = genreRepository;
        _validationService = validationService;
    }

    public async Task<List<GetGamesDto>> GetAllGamesAsync()
    {
        var games = await _gameRepository.GetAllAsync();
        return games.Select(g => g.EntityToDto()).ToList();
    }

    public async Task<GetGamesDto?> GetGameByIdAsync(int id)
    {
        var game = await _gameRepository.GetByIdAsync(id) ?? throw new NotFoundException("game", id);
        return game?.EntityToDto();
    }

    public async Task<List<GetGamesDto>> GetGamesByNameAsync(string name)
    {
        var games = await _gameRepository.GetByNameAsync(name);
        return games.Select(g => g.EntityToDto()).ToList();
    }

    public async Task<GetGamesDto> CreateGameAsync(CreateGameDto newGame)
    {
        await _validationService.ValidateAsync(newGame);

        var genre = await _genreRepository.GetByIdAsync(newGame.GenreId) ?? throw new NotFoundException("Genre", newGame.GenreId);

        var game = newGame.DtoToEntity();
        game.Genre = genre;

        await _gameRepository.CreateAsync(game);

        return game.EntityToDto();
    }

    public async Task<bool> DeleteGameAsync(int id)
    {
        var game = await _gameRepository.GetByIdAsync(id);
        if (game == null)
        {
            return false;
        }

        await _gameRepository.DeleteAsync(game);
        return true;
    }

    public async Task<GetGamesDto> UpdateGameAsync(int id, UpdateGameDto updatedGame)
    {
        await _validationService.ValidateAsync(updatedGame);

        var game = await _gameRepository.GetByIdAsync(id) ?? throw new NotFoundException("Game", id);
        var genre = await _genreRepository.GetByIdAsync(updatedGame.GenreId) ?? throw new NotFoundException("Genre", updatedGame.GenreId);

        game.Name = updatedGame.Name;
        game.Genre = genre;
        game.Price = updatedGame.Price;
        game.ReleaseDate = updatedGame.ReleaseDate;
        
        await _gameRepository.UpdateAsync(game);
        return game.EntityToDto();
    }

    public async Task<PaginationResponse<GetGamesDto>> GetPagedGamesAsync(PaginationRequest paginationRequest)
    {
        var (games, totalCount) = await _gameRepository.GetPagedAsync(paginationRequest.Skip, paginationRequest.PageSize);
        var items = games.Select(g => g.EntityToDto()).ToList();
        
        return new PaginationResponse<GetGamesDto>
        {
            Items = items,
            PageNumber = paginationRequest.PageNumber,
            PageSize = paginationRequest.PageSize,
            TotalCount = totalCount
        };
    }
}
