// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     DependencyInjection.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : TailwindBlog
// Project Name :  Persistence.MongoDb
// =======================================================

namespace Persistence;

/// <summary>
///   Provides extension methods for registering MongoDB persistence services.
/// </summary>
public static class DependencyInjection
{

	/// <summary>
	///   Adds MongoDB persistence and related services to the DI container.
	/// </summary>
	/// <param name="services">The service collection.</param>
	/// <returns>The updated service collection.</returns>
	public static IServiceCollection AddPersistence(this IServiceCollection services)
	{

		ArgumentNullException.ThrowIfNull(services);

		// Only get connection string from environment variable
		var connectionString = Environment.GetEnvironmentVariable("MongoDb__ConnectionString")
													?? throw new InvalidOperationException("MongoDB connection string is not configured.");

		var mongoSettings = new DatabaseSettings(connectionString, DatabaseName);

		services.AddSingleton<IDatabaseSettings>(mongoSettings);

		// Register memory cache for CacheService
		services.AddMemoryCache();

		// Register other services
		services.AddSingleton<IMongoDbContextFactory, MongoDbContextFactory>();

		services.AddScoped<IArticleRepository, ArticleRepository>();
		services.AddScoped<ICategoryRepository, CategoryRepository>();

		services.AddScoped<ICacheService, CacheService>();

		services.AddScoped<IArticleService, ArticleService>();
		services.AddScoped<ICategoryService, CategoryService>();

		return services;

	}

}