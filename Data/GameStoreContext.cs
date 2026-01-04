
namespace GameStoreControllerApi.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
{
    public DbSet<Game> games => Set<Game>();
    public DbSet<Genre> genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = 1, Name = "Action" },
            new Genre { Id = 2, Name = "Adventure" },
            new Genre { Id = 3, Name = "RPG" },
            new Genre { Id = 4, Name = "Strategy" },
            new Genre { Id = 5, Name = "Sports" },
            new Genre { Id = 6, Name = "Racing" }
        );
    }
}
