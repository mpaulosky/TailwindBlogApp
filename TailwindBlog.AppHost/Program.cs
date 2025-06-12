using static TailwindBlog.Domain.Constants.ServiceNames;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis(OutputCache);

var mongoServer = builder.AddMongoDB(ServerName)
		.WithLifetime(ContainerLifetime.Persistent)
		.WithMongoExpress();

var mongoDb = mongoServer.AddDatabase(DatabaseName);

builder.AddProject<Projects.TailwindBlog_Web>(WebApp)
		.WithExternalHttpEndpoints()
		.WithHealthCheck("/health")
		.WithReference(cache)
		.WaitFor(cache)
		.WithReference(mongoDb)
		.WaitFor(mongoDb);

builder.Build().Run();