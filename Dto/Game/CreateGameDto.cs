
namespace GameStoreControllerApi.Dto.Game;

public record class CreateGameDto(
    
    string Name, 
    int GenreId,
    decimal Price,
    DateOnly ReleaseDate
);
