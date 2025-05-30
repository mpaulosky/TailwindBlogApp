// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     MongoDbContextFactory.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb
// =============================================

namespace TailwindBlog.Persistence;

/// <summary>
///   MongoDbContext class
/// </summary>
public class MongoDbContextFactory : IMongoDbContextFactory
{
	private readonly IMongoClient _client;
	private readonly IMongoDatabase _database;

	/// <summary>
	///   MongoDbContextFactory constructor
	/// </summary>
	/// <param name="settings">IDatabaseSettings</param>
	/// <param name="client">IMongoClient</param>
	public MongoDbContextFactory(IDatabaseSettings settings, IMongoClient? client = null)
	{
		ConnectionString = settings.ConnectionStrings;
		DbName = settings.DatabaseName;
		_client = client ?? new MongoClient(ConnectionString);
		_database = _client.GetDatabase(DbName);
	}

	/// <summary>
	///   Gets the database.
	/// </summary>
	/// <value>
	///   The database.
	/// </value>
	public IMongoDatabase Database => _database;

	/// <summary>
	///   Gets the client.
	/// </summary>
	/// <value>
	///   The client.
	/// </value>
	public IMongoClient Client => _client;

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
		ArgumentException.ThrowIfNullOrEmpty(name);
		return _database.GetCollection<T>(name);
	}
}