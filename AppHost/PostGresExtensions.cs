// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     PostgresExtensions.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  AppHost
// =======================================================

using Projects;

namespace AppHost;

/// <summary>
///   Extension methods for configuring PostgresSQL services in a distributed application.
/// </summary>
public static class PostgresExtensions
{

	/// <summary>
	///   Adds PostgresSQL database services to the distributed application.
	/// </summary>
	/// <param name="builder">The distributed application builder.</param>
	/// <param name="testOnly">When true, configures a database for testing purposes only.</param>
	/// <returns>A tuple containing the database resource and migration service resource.</returns>
	public static
			(IResourceBuilder<PostgresDatabaseResource> db,
			IResourceBuilder<ProjectResource> migrationSvc) AddPostgresServices(
					this IDistributedApplicationBuilder builder,
					bool testOnly = false)
	{

		var dbServer = builder.AddPostgres(ServerName)
				.WithImageTag(Versions.Postgres);

		if (!testOnly)
		{

			dbServer = dbServer
					.WithLifetime(ContainerLifetime.Persistent)
					.WithDataVolume($"{ServerName}-test-data")
					.WithPgAdmin(config =>
					{
						config.WithImageTag(Versions.Pgadmin);
						config.WithLifetime(ContainerLifetime.Persistent);
					});

		}
		else
		{
			dbServer = dbServer
					.WithLifetime(ContainerLifetime.Session)
					.WithDataVolume($"{ServerName}-data");
		}

		var outdbrd = dbServer.AddDatabase(DatabaseName);

		var migrationSvc = builder
				.AddProject<Persistence_Postgres_Migrations>(
						$"{DatabaseName}migrationsvc")
				.WithReference(outdbrd)
				.WaitFor(dbServer);

		return (outdbrd, migrationSvc);

	}

	/// <summary>
	///   A collection of version information used by the containers in this app
	/// </summary>
	private static class Versions
	{

		/// <summary>
		///   The PostgresSQL server version.
		/// </summary>
		public const string Postgres = "17.2";

		/// <summary>
		///   The pgAdmin version tag.
		/// </summary>
		public const string Pgadmin = "latest";

	}

}