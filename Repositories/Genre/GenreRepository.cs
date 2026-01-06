
namespace GameStoreControllerApi.Repositories.Genre;

public class GenreRepository : IGenreRepository
{
    public readonly GameStoreContext _dbContext;
    public GenreRepository(GameStoreContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Entities.Genre?> GetByIdAsync(int id)
    {
        return await _dbContext.genres.FindAsync(id);
    }
}
