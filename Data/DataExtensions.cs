namespace GameStoreControllerApi.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope(); //with this we basically ask the asp.net to give us scopes of the services registered in our applicaiton
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>(); //we extract our required dbcontext scope from the list of scopes
        dbContext.Database.Migrate(); //this will apply any pending migrations for the context to the database. Will create the database if it does not already exist.
    }
}
