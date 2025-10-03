using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShowcaseGallery.Api.Data;
using ShowcaseGallery.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Configure database connection
var useHerokuDb = builder.Configuration.GetValue<bool>("USE_HEROKU_DB");
string connectionString;

if (useHerokuDb)
{
    // Get Heroku DATABASE_URL and convert to Npgsql format
    var herokuConnectionUrl = builder.Configuration.GetConnectionString("Heroku");
    
    if (string.IsNullOrEmpty(herokuConnectionUrl))
    {
        throw new InvalidOperationException("Heroku connection string not configured. Check ConnectionStrings:Heroku in appsettings.json or HEROKU_DATABASE_URL environment variable.");
    }
    
    connectionString = ConvertHerokuConnectionString(herokuConnectionUrl);
}
else
{
    connectionString = builder.Configuration.GetConnectionString("Default")
        ?? throw new InvalidOperationException("Default connection string not configured");
}

// Log which database is being used
builder.Logging.AddConsole();
Console.WriteLine($"Database Mode: {(useHerokuDb ? "Heroku Cloud" : "Local Docker")}");
Console.WriteLine($"Connection String: {MaskConnectionString(connectionString)}");

// Add Entity Framework Core with PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add JWT Authentication
var jwtSecret = builder.Configuration["Jwt:Secret"] 
    ?? throw new InvalidOperationException("JWT Secret not configured");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] 
    ?? throw new InvalidOperationException("JWT Issuer not configured");
var jwtAudience = builder.Configuration["Jwt:Audience"] 
    ?? throw new InvalidOperationException("JWT Audience not configured");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Register application services
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();

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

// Apply migrations automatically for Heroku
if (useHerokuDb)
{
    try
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        
        Console.WriteLine("Applying database migrations...");
        db.Database.Migrate();
        Console.WriteLine("Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error applying migrations: {ex.Message}");
        throw;
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health check endpoint
app.MapGet("/health", () => Results.Ok(new
{
    status = "healthy",
    timestamp = DateTime.UtcNow,
    environment = app.Environment.EnvironmentName,
    database = useHerokuDb ? "Heroku PostgreSQL" : "Local PostgreSQL"
}))
.WithName("HealthCheck");

app.Run();

// Helper method to convert Heroku connection string to Npgsql format
static string ConvertHerokuConnectionString(string herokuConnectionUrl)
{
    // Heroku format: postgres://user:password@host:port/database
    // Npgsql format: Host=host;Port=port;Database=database;Username=user;Password=password;SSL Mode=Require;Trust Server Certificate=true
    
    var uri = new Uri(herokuConnectionUrl);
    var userInfo = uri.UserInfo.Split(':');
    
    return $"Host={uri.Host};Port={uri.Port};Database={uri.LocalPath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};SSL Mode=Require;Trust Server Certificate=true";
}

// Helper method to mask sensitive information in connection string for logging
static string MaskConnectionString(string connectionString)
{
    var parts = connectionString.Split(';');
    var masked = parts.Select(part =>
    {
        if (part.StartsWith("Password=", StringComparison.OrdinalIgnoreCase))
        {
            return "Password=***";
        }
        return part;
    });
    return string.Join(";", masked);
}