var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.TailwindBlog_ApiService>("apiservice")
    .WithHttpsHealthCheck("/health");

builder.AddProject<Projects.TailwindBlog_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpsHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
