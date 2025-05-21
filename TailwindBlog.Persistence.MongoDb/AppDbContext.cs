// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     AppDbContext.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb
// =======================================================

using Microsoft.Extensions.Logging;

namespace TailwindBlog.Persistence;

/// <summary>
/// MongoDB implementation of the application database context
/// </summary>
public sealed class AppDbContext : IApplicationDbContext
{
	private readonly IMongoDatabase _database;

	/// <summary>
	/// Initializes a new instance of the <see cref="AppDbContext"/> class
	/// </summary>
	/// <param name="database">The MongoDB database instance</param>
	public AppDbContext(IMongoDatabase database)
	{
		_database = database ?? throw new ArgumentNullException(nameof(database));
	}

	/// <inheritdoc/>
	public IMongoCollection<Article> Articles => _database.GetCollection<Article>("articles");

	/// <inheritdoc/> 
	public IMongoCollection<Category> Categories => _database.GetCollection<Category>("categories");
}