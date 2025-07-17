// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     RegisterPostgresServices.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Persistence.Postgres
// =======================================================

namespace Persistence.Postgres;

public class RegisterPostgresServices : IRegisterServices
{

	public IHostApplicationBuilder RegisterServices(IHostApplicationBuilder host, bool disableRetry = false)
	{

		// Set the database name from Constants
		var databaseName = DatabaseName;

		host.AddNpgsqlDbContext<PgContext>(databaseName, configure =>
		{
			configure.DisableRetry = disableRetry;
		});

		// Register services
		host.Services.AddScoped<ICacheService, CacheService>();
		host.Services.AddScoped<IArticleService, ArticleService>();
		host.Services.AddScoped<ICategoryService, CategoryService>();

		return host;

	}

}