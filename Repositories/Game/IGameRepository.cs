using Entity = GameStoreControllerApi.Entities;
namespace GameStoreControllerApi.Repositories.Game;

public interface IGameRepository
{
    Task<List<Entity.Game>> GetAllAsync();
    Task<Entity.Game?> GetByIdAsync(int id);
    Task<List<Entity.Game>> GetByNameAsync(string name);
    Task CreateAsync(Entity.Game game);
    Task UpdateAsync(Entity.Game game);
    Task DeleteAsync(Entity.Game game);

    Task<(List<Entity.Game> Items, int TotalCount)> GetPagedAsync(int skip, int take);
}
