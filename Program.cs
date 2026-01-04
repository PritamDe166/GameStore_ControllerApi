
var builder = WebApplication.CreateBuilder(args);

//Adding Services
var conString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(conString);

//Adding Controllers
builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssemblyContaining<CreateGameValidatorServiceValidations>();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();


var app = builder.Build();

app.UseExceptionHandler();

//mapping controllers
app.MapControllers();

app.MigrateDb();
app.Run();
