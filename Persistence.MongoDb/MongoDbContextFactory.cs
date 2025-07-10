// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     MongoDbContextFactory.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : TailwindBlog
// Project Name :  Persistence.MongoDb
// =============================================

namespace Persistence;

/// <summary>
///   MongoDbContext class
/// </summary>
public class MongoDbContextFactory : IMongoDbContextFactory
{

	/// <summary>
	///   MongoDbContextFactory constructor
	/// </summary>
	/// <param name="settings">IDatabaseSettings</param>
	/// <param name="client">IMongoClient</param>
	public MongoDbContextFactory(IDatabaseSettings settings, IMongoClient? client = null)
	{
		ConnectionString = settings.ConnectionStrings;
		DbName = settings.DatabaseName;
		Client = client ?? new MongoClient(ConnectionString);
		Database = Client.GetDatabase(DbName);
	}

	/// <summary>
	///   Gets the database.
	/// </summary>
	/// <value>
	///   The database.
	/// </value>
	public IMongoDatabase Database { get; }

	/// <summary>
	///   Gets the client.
	/// </summary>
	/// <value>
	///   The client.
	/// </value>
	public IMongoClient Client { get; }

	/// <summary>
	///   Gets the connection string.
	/// </summary>
	/// <value>
	///   The connection string.
	/// </value>
	public string ConnectionString { get; }

	/// <summary>
	///   Gets the name of the database.
	/// </summary>
	/// <value>
	///   The name of the database.
	/// </value>
	public string DbName { get; }

	/// <summary>
	///   GetCollection method
	/// </summary>
	/// <param name="name">string collection name</param>
	/// <typeparam name="T">The Entity Name cref="CategoryModel"</typeparam>
	/// <returns>IMongoCollection</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public IMongoCollection<T> GetCollection<T>(string? name)
	{

		if (string.IsNullOrEmpty(name))
		{

			throw new ArgumentNullException(nameof(name), "Collection name cannot be null or empty.");

		}

		var result = Database.GetCollection<T>(name);

		if (result is not  { } collection)
		{

			throw new InvalidOperationException($"Failed to retrieve collection '{name}' of type '{typeof(T).Name}'.");

		}

		return result;

	}

}