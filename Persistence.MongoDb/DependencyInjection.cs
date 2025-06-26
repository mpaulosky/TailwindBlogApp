// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     DependencyInjection.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : TailwindBlog
// Project Name :  Persistence.MongoDb
// =======================================================

using Persistence.Repositories;
using Persistence.Services;

namespace Persistence;

public static class DependencyInjection
{

	public static IServiceCollection AddPersistence(
			this IServiceCollection services,
			IConfiguration configuration)
	{

		var connectionString = configuration.GetConnectionString(DatabaseName) ??
													throw new InvalidOperationException(
															$"Connection string '{DatabaseName}' not found.");

		var mongoSettings = new DatabaseSettings(connectionString, DatabaseName);

		services.AddSingleton<IDatabaseSettings>(mongoSettings);

		services.AddSingleton<IMongoDbContextFactory, MongoDbContextFactory>();

		services.AddScoped<IArticleRepository, ArticleRepository>();
		services.AddScoped<ICategoryRepository, CategoryRepository>();
		services.AddScoped<IArticleService, ArticleService>();
		services.AddScoped<ICategoryService, CategoryService>();

		return services;

	}

}