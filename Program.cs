
var builder = WebApplication.CreateBuilder(args);

// Infrastructure
var connectionString = builder.Configuration.GetConnectionString("GameStore");
builder.Services.AddSqlite<GameStoreContext>(connectionString);

// Application Services
builder.Services.AddScoped<IGameService, GameService>();

// MVC / API
builder.Services.AddControllers();

// Validation & Error Handling
builder.Services.AddValidatorsFromAssemblyContaining<CreateGameValidatorServiceValidations>();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();

var app = builder.Build();

// Middleware Pipeline
app.UseExceptionHandler();

app.MapControllers();

// Startup Tasks
app.MigrateDb();

app.Run();

