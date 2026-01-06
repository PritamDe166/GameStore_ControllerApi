namespace GameStoreControllerApi.Dto.Game;

public record class UpdateGameDto(
    string Name, 
    int GenreId,
    decimal Price,
    DateOnly ReleaseDate
);
