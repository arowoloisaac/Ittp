var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.aton_intern>("aton-intern");

builder.Build().Run();
