namespace GameStoreControllerApi.Dto.Game;

public record class GetGamesDto(
    int Id, 
    string Name, 
    int GenreId, 
    string GenreName, 
    decimal Price,
    DateOnly ReleaseDate
);


