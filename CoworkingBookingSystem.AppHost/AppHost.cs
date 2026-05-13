var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Users_API>("users-api");
builder.AddProject<Projects.Coworking_API>("coworking-api");
builder.AddProject<Projects.Gateway_API>("gateway-api");

builder.Build().Run();
