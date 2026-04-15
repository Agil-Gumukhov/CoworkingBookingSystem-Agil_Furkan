var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Coworking_API>("coworking-api");

builder.Build().Run();
