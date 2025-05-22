// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     DependencyInjection.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb
// =======================================================

namespace TailwindBlog.Persistence;

public static class DependencyInjection
{

	public static IServiceCollection AddPersistence(
			this IServiceCollection services,
			IConfiguration configuration)
	{
		var connectionString = configuration.GetConnectionString(DatabaseName) ??
													throw new InvalidOperationException(
															$"Connection string '{DatabaseName}' not found.");

		var client = new MongoClient(connectionString);
		var database = client.GetDatabase(DatabaseName);

		services.AddSingleton(database);
		services.AddScoped<IApplicationDbContext, AppDbContext>();
		services.AddScoped<IArticleRepository, ArticleRepository>();
		services.AddScoped<ICategoryRepository, CategoryRepository>();

		return services;
	}

}