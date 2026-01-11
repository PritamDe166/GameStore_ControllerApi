
using GameStoreControllerApi.Entities;

namespace GameStoreControllerApi.Repositories.Game;

public class GameRepository : IGameRepository
{
    public readonly GameStoreContext _dbContext;
    public GameRepository(GameStoreContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Entities.Game>> GetAllAsync()
    {
        return await _dbContext.games
                               .Include(g => g.Genre)
                               .ToListAsync();
    }

    public async Task<Entities.Game?> GetByIdAsync(int id)
    {
        return await _dbContext.games
                                .Include(g => g.Genre)
                                .Where(g => g.Id == id)
                                .FirstOrDefaultAsync();
    }

    public async Task<List<Entities.Game>> GetByNameAsync(string name)
    {
        return await _dbContext.games
                                .Include(g => g.Genre)
                                .Where(g => g.Name == name)
                                .ToListAsync();
    }

    public async Task CreateAsync(Entities.Game newGame)
    {
        _dbContext.games.Add(newGame);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Entities.Game game)
    {
        _dbContext.games.Remove(game);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Entities.Game game)
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<(List<Entities.Game> Items, int TotalCount)> GetPagedAsync(int skip, int take)
    {
        var query = _dbContext.games
            .Include(g => g.Genre)
            .AsNoTracking();

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return (items, totalCount);
    }
}
