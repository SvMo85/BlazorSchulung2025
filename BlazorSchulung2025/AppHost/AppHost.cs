using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<BlazorSchulung2025>("blazor-schulungsprojekt");

builder.Build().Run();