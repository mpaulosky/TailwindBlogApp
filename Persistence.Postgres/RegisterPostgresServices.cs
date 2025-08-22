// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     RegisterPostgresServices.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Persistence.Postgres
// =======================================================

using Persistence.Postgres.Repositories;

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
		host.Services.AddMemoryCache();
		host.Services.AddSingleton<ICacheService, CacheService>();
		host.Services.AddTransient<IArticleRepository, ArticleRepository>();
		host.Services.AddTransient<ICategoryRepository, CategoryRepository>();
		host.Services.AddScoped<IArticleService, ArticleService>();
		host.Services.AddScoped<ICategoryService, CategoryService>();

		return host;

	}

}