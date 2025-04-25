using static TailwindBlog.Domain.Constants.ServiceNames;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis(OutputCache);

var mongoServer = builder.AddMongoDB(ServerName)
		.WithLifetime(ContainerLifetime.Persistent)
		.WithMongoExpress();

var mongoDb = mongoServer.AddDatabase(DatabaseName);

var apiService = builder.AddProject<Projects.TailwindBlog_ApiService>(ApiService)
		.WithHttpsHealthCheck("/health")
		.WithReference(mongoDb)
		.WaitFor(mongoDb);

builder.AddProject<Projects.TailwindBlog_Web>(WebApp)
		.WithExternalHttpEndpoints()
		.WithHttpsHealthCheck("/health")
		.WithReference(apiService)
		.WaitFor(apiService)
		.WithReference(cache)
		.WaitFor(cache);

builder.Build().Run();
