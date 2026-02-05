// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Program.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  AppHost
// =======================================================

using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var pgServer = builder.AddPostgres(ServerName)
		.WithLifetime(ContainerLifetime.Persistent)
		.WithDataVolume($"{ServerName}-test-data")
		.WithPgAdmin(config =>
		{
			config.WithImageTag("latest");
			config.WithLifetime(ContainerLifetime.Persistent);
		});

var db = pgServer.AddDatabase(DatabaseName);

builder.AddProject<Web>(WebApp)
		.WithReference(db).WaitFor(db)
		.WithExternalHttpEndpoints();

builder.Build().Run();