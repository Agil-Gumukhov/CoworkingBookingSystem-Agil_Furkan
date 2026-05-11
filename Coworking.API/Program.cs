using Coworking.APP.Domain;
using Coworking.APP.Services;
using Microsoft.EntityFrameworkCore;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// -----------------------------------------------------------------------------------
// Add services to the IoC (Inversion of Control) container for Dependency Injections.
// -----------------------------------------------------------------------------------

/// <summary>
/// Registers the application's DbContext (named 'CoworkingDb') with the dependency injection container.
/// Configures the DbContext to use SQLite as the database provider.
/// The connection string named "CoworkingDb" is retrieved from the application's configuration settings (appsettings.json).
/// This setup enables the application to connect to the specified SQLite database when interacting with entity sets.
/// Whenever a DbContext injection occurs through the constructor of a class (such as a service class),
/// initialize an object of type CoworkingDb and use this object in the class for database operations.
/// </summary>
builder.Services.AddDbContext<CoworkingDb>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("CoworkingDb")));

/// <summary>
/// Registers MediatR services with the dependency injection container.
/// MediatR is a popular .NET library that implements the mediator pattern, enabling decoupled communication
/// between components by sending requests (commands, queries, events) to handlers without direct dependencies.
/// This configuration scans the assembly containing the 'CoworkingDb' type for any classes that implement
/// MediatR handler interfaces (such as IRequestHandler, INotificationHandler, etc.).
/// This allows automatic discovery and registration of all MediatR handlers in the specified assembly.
/// </summary>
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CoworkingDb).Assembly));

/// <summary>
/// Registers application services with appropriate lifetimes.
/// Scoped: One instance per HTTP request (suitable for stateful services like BranchService, RoomService, etc.)
/// </summary>
builder.Services.AddScoped<BranchService>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<DeskService>();
builder.Services.AddScoped<BookingService>();

/// <summary>
/// Registers the IHttpContextAccessor service with the dependency injection container.
/// This service allows access to the current HttpContext (such as request headers, user identity, etc.)
/// from non-controller classes via constructor injection.
/// </summary>
builder.Services.AddHttpContextAccessor();

/// <summary>
/// Registers the IHttpClientFactory service and enables dependency injection for HttpClient instances.
/// This allows the application to create and manage HttpClient objects efficiently.
/// </summary>
builder.Services.AddHttpClient();

/// <summary>
/// Adds controller support for handling HTTP API requests.
/// </summary>
builder.Services.AddControllers();

/// <summary>
/// Adds support for API endpoint discovery and OpenAPI/Swagger documentation generation.
/// </summary>
builder.Services.AddEndpointsApiExplorer();

/// <summary>
/// Configure Swagger/OpenAPI documentation.
/// </summary>
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Coworking Booking System API",
        Version = "v1"
    });
});

/// <summary>
/// Registers and configures CORS services for the application.
/// The configuration below adds a default CORS policy that allows requests from any origin.
/// </summary>
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder => builder
        .AllowAnyOrigin()   // Allows requests from any domain.
        .AllowAnyHeader()   // Allows any HTTP headers in the request.
        .AllowAnyMethod()); // Allows any HTTP method (GET, POST, PUT, DELETE, etc.).
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.Run();
