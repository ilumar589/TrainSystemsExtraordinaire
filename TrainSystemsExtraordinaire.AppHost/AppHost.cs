var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.WebCCTVUi>("webcctvui");

builder.Build().Run();
