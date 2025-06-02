// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     TestFixtures.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb.Tests.Unit
// =============================================

namespace TailwindBlog.Persistence;

[ExcludeFromCodeCoverage]
public static class TestFixtures
{
	public static Mock<IAsyncCursor<TEntity>> GetMockCursor<TEntity>(IEnumerable<TEntity> list) where TEntity : class?
	{
		Mock<IAsyncCursor<TEntity>> cursor = new();
		cursor.Setup(a => a.Current).Returns(list);
		cursor
			.SetupSequence(a => a.MoveNext(It.IsAny<CancellationToken>()))
			.Returns(true)
			.Returns(false);
		cursor
			.SetupSequence(a => a.MoveNextAsync(It.IsAny<CancellationToken>()))
			.Returns(Task.FromResult(true))
			.Returns(Task.FromResult(false));
		return cursor;
	}

	public static Mock<IMongoCollection<TEntity>> GetMockCollection<TEntity>(Mock<IAsyncCursor<TEntity>> cursor)
		where TEntity : class?
	{
		Mock<IMongoCollection<TEntity>> collection = new() { Name = CollectionNames.GetCollectionName(nameof(TEntity)) };

		collection.Setup(op =>
				op.FindAsync
				(
					It.IsAny<FilterDefinition<TEntity>>(),
					It.IsAny<FindOptions<TEntity, TEntity>>(),
					It.IsAny<CancellationToken>()
				))
			.ReturnsAsync(cursor.Object);

		collection.Setup(op =>
			op.InsertOneAsync
			(
				It.IsAny<TEntity>(),
				It.IsAny<InsertOneOptions>(),
				It.IsAny<CancellationToken>()
			)).Returns(Task.CompletedTask);

		return collection;
	}

	public static Mock<IMongoDbContextFactory> GetMockContext()
	{
		Mock<IMongoClient> mockClient = new();
		Mock<IMongoDatabase> mockDatabase = new();
		Mock<IMongoDbContextFactory> context = new();
		Mock<IClientSessionHandle> mockSession = new();
		context.Setup(op => op.Client).Returns(mockClient.Object);
		context.Setup(op => op.Database).Returns(mockDatabase.Object);
		context.Setup(op =>
				op.Client.StartSessionAsync(
					It.IsAny<ClientSessionOptions>(),
					It.IsAny<CancellationToken>()))
			.Returns(Task.FromResult(mockSession.Object));

		return context;
	}

	public static Mock<IMongoDbContextFactory> GetMockContextWithOutDataBase()
	{
		Mock<IMongoClient> mockClient = new();
		Mock<IMongoDbContextFactory> context = new();
		Mock<IClientSessionHandle> mockSession = new();
		context.Setup(op => op.Client).Returns(mockClient.Object);
		context.Setup(op =>
				op.Client.StartSessionAsync(
					It.IsAny<ClientSessionOptions>(),
					It.IsAny<CancellationToken>()))
			.Returns(Task.FromResult(mockSession.Object));

		return context;
	}

	public static DatabaseSettings Settings()
	{
		const string connectionStrings = "mongodb://test123";
		const string databaseName = "TestDb";

		DatabaseSettings settings = new(connectionStrings, databaseName)
		{
			ConnectionStrings = connectionStrings, DatabaseName = databaseName
		};

		return settings;
	}

	public static IOptions<DatabaseSettings> Settings(string connectionStrings, string databaseName)
	{
		DatabaseSettings settings = new(connectionStrings, databaseName)
		{
			ConnectionStrings = connectionStrings, DatabaseName = databaseName
		};

		return Options.Create(settings);
	}
}