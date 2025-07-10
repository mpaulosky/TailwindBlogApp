using Projects;

var serverName = ServerName;
var databaseName = DatabaseName;
var outputCacheName = OutputCache;

var builder = DistributedApplication.CreateBuilder(args);

var outputCache = builder.AddRedis(outputCacheName);

// Add a default admin user and password as parameters
var adminPassword = builder.AddParameter("AdminPassword", true);
var adminUser = builder.AddParameter("AdminUser");

// Add environment variable for the MongoDB connection string
var mongoConnectionString = builder.AddParameter("MongoDbConnectionString", true);

// Create a MongoDB server for articles
var articleServer = builder.AddMongoDB(serverName)
		.WithDataVolume()
		.WithMongoExpress()
		.WithEnvironment("MONGO_INITDB_ROOT_USERNAME", adminUser)
		.WithEnvironment("MONGO_INITDB_ROOT_PASSWORD", adminPassword)
		.WithEnvironment("MongoDb__ConnectionString", mongoConnectionString)
		.WithLifetime(ContainerLifetime.Persistent);

// Create a database for articles
var articleDatabase = articleServer.AddDatabase(databaseName);

// Create a web project
builder.AddProject<Web>("WebApp")
		.WithExternalHttpEndpoints()
		.WithReference(outputCache)
		.WaitFor(outputCache)
		.WaitFor(articleServer)
		.WaitFor(articleDatabase);

builder.Build().Run();