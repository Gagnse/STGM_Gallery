var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

// Configure database connection
var useHerokuDb = builder.Configuration.GetValue<bool>("USE_HEROKU_DB");
var connectionString = useHerokuDb
    ? builder.Configuration.GetConnectionString("Heroku")
    : builder.Configuration.GetConnectionString("Default");

// Log which database is being used (helpful for debugging)
builder.Logging.AddConsole();
Console.WriteLine($"Database Mode: {(useHerokuDb ? "Heroku Cloud" : "Local Docker")}");
Console.WriteLine($"Connection String: {connectionString?.Substring(0, Math.Min(50, connectionString?.Length ?? 0))}...");

// TODO: Add database context here when implementing EF Core
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseNpgsql(connectionString));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://frontend:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    timestamp = DateTime.UtcNow,
    environment = app.Environment.EnvironmentName,
    database = useHerokuDb ? "Heroku PostgreSQL" : "Local PostgreSQL"
}))
.WithName("HealthCheck");

// Test endpoint
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}