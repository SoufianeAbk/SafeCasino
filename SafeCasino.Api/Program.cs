using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SafeCasino.Api.Services;
using SafeCasino.Data.Data;
using SafeCasino.Data.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// ✨ Add Swagger/OpenAPI (vervangt AddOpenApi)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SafeCasino API",
        Version = "v1",
        Description = "REST API voor SafeCasino - Online Casino Platform",
        Contact = new OpenApiContact
        {
            Name = "SafeCasino Support",
            Email = "support@safecasino.be"
        }
    });

    // Add Authentication support in Swagger UI
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configure Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity — use ApplicationRole (match ApplicationDbContext)
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 8;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;

    // Email confirmation required
    options.SignIn.RequireConfirmedEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure application cookie for API
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    options.SlidingExpiration = true;

    // API-specific: return 401/403 instead of redirects
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

// Configure SMTP Settings
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// Configure Email Service
builder.Services.AddScoped<IEmailService, EmailService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebApp",
        builder => builder
            .WithOrigins("https://localhost:7243", "http://localhost:5136") // Web app URLs
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

    // ✨ Enable Swagger UI
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "SafeCasino API v1");
        options.RoutePrefix = "swagger"; // Swagger UI op /swagger
        options.DocumentTitle = "SafeCasino API Documentation";
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowWebApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ✨ Add Health Check endpoint
app.MapGet("/health", () => Results.Ok(new
{
    Status = "Healthy",
    Timestamp = DateTime.UtcNow,
    Environment = app.Environment.EnvironmentName
}))
.WithName("HealthCheck")
.WithTags("Health");

// ✨ Add API Info endpoint
app.MapGet("/api", () => Results.Ok(new
{
    Message = "SafeCasino API is running",
    Version = "v1.0",
    Endpoints = new
    {
        Documentation = "/swagger",
        Health = "/health",
        Auth = "/api/auth",
        Games = "/api/games",
        Admin = "/api/admin",
        Casino = "/api/casino",
        User = "/api/user"
    }
}))
.WithName("ApiInfo")
.WithTags("Root");

app.Run();