using Entity = GameStoreControllerApi.Entities;

namespace GameStoreControllerApi.Repositories.Genre;

public interface IGenreRepository
{
    Task<Entity.Genre?> GetByIdAsync(int id);
}
