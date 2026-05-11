using Coworking.APP.Domain;
using Coworking.APP.Services;
using Microsoft.EntityFrameworkCore;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<CoworkingDb>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("CoworkingDb")));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CoworkingDb).Assembly));

// Register Services (Business Logic Layer)
builder.Services.AddScoped<BranchService>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<DeskService>();
builder.Services.AddScoped<BookingService>();


var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
