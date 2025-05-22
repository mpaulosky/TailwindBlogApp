// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArticleRepositoryTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb.Tests.Unit
// =======================================================

using TailwindBlog.Persistence.Repositories.TestData;

namespace TailwindBlog.Persistence.Repositories;

[ExcludeFromCodeCoverage]
public sealed class ArticleRepositoryTests
{

	private readonly IMongoDatabase _database;

	private readonly IMongoCollection<Article> _collection;

	private readonly ArticleRepository _sut;

	private readonly AppUserModel _testUser;

	private readonly Article _testArticle;

	private readonly List<Article> _testArticles;

	public ArticleRepositoryTests()
	{
		_database = Substitute.For<IMongoDatabase>();
		_collection = Substitute.For<IMongoCollection<Article>>();
		_database.GetCollection<Article>("articles").Returns(_collection);

		_sut = new ArticleRepository(_database);

		// Test data setup
		_testUser = new AppUserModel { Id = "test-user" };
		_testArticle = ArticleTestDataBuilder.CreateDefault();

		_testArticles =
		[
				new ArticleTestDataBuilder().WithAuthor(_testUser).Build(),
				new ArticleTestDataBuilder().WithAuthor(_testUser).Build(),
				new ArticleTestDataBuilder().WithAuthor(new AppUserModel { Id = "different-user" }).Build()
		];
	}

	[Fact]
	public void Add_ShouldInsertOneArticle()
	{
		// Act
		_sut.Add(_testArticle);

		// Assert
		_collection.Received(1).InsertOne(
				Arg.Is<Article>(a => a.Id == _testArticle.Id),
				Arg.Any<InsertOneOptions>(),
				Arg.Any<CancellationToken>()
		);
	}

	[Fact]
	public async Task GetById_ShouldReturnArticle()
	{
		// Arrange
		var cursor = Substitute.For<IAsyncCursor<Article>>();
		cursor.MoveNextAsync().Returns(true, false);
		cursor.Current.Returns([ _testArticle ]);

		_collection.FindAsync(
				Arg.Any<FilterDefinition<Article>>(),
				Arg.Any<FindOptions<Article>>(),
				Arg.Any<CancellationToken>()
		).Returns(cursor);

		// Act
		var result = await _sut.GetByIdAsync(_testArticle.Id);

		// Assert
		result.Should().NotBeNull();
		result.Should().BeEquivalentTo(_testArticle);
	}

	[Fact]
	public async Task GetById_WhenArticleNotFound_ShouldReturnNull()
	{
		// Arrange
		var cursor = Substitute.For<IAsyncCursor<Article>>();
		cursor.MoveNextAsync().Returns(false);

		_collection.FindAsync(
				Arg.Any<FilterDefinition<Article>>(),
				Arg.Any<FindOptions<Article>>(),
				Arg.Any<CancellationToken>()
		).Returns(cursor);

		// Act
		var result = await _sut.GetByIdAsync(ObjectId.GenerateNewId());

		// Assert
		result.Should().BeNull();
	}

	[Fact]
	public void Update_ShouldUpdateArticle()
	{
		// Act
		_sut.Update(_testArticle);

		// Assert
		_collection.Received(1).ReplaceOne(
				Arg.Any<FilterDefinition<Article>>(),
				Arg.Is<Article>(a => a.Id == _testArticle.Id),
				Arg.Any<ReplaceOptions>(),
				Arg.Any<CancellationToken>()
		);
	}

	[Fact]
	public void Remove_ShouldDeleteArticle()
	{
		// Act
		_sut.Remove(_testArticle);

		// Assert
		_collection.Received(1).DeleteOne(
				Arg.Any<FilterDefinition<Article>>(),
				Arg.Any<CancellationToken>()
		);
	}

	[Fact]
	public async Task GetByUserAsync_ShouldReturnArticlesForUser()
	{
		// Arrange
		var userArticles = _testArticles.Where(a => a.Author.Id == _testUser.Id).ToList();
		var cursor = Substitute.For<IAsyncCursor<Article>>();
		cursor.MoveNextAsync().Returns(true, false);
		cursor.Current.Returns(userArticles);

		_collection.FindAsync(
				Arg.Any<FilterDefinition<Article>>(),
				Arg.Any<FindOptions<Article>>(),
				Arg.Any<CancellationToken>()
		).Returns(cursor);

		// Act
		var result = await _sut.GetByUserAsync(_testUser.Id);

		// Assert
		result.Should().NotBeNull();
		result.Should().HaveCount(2);
		result.Should().OnlyContain(a => a.Author.Id == _testUser.Id);
	}

	[Fact]
	public async Task GetByUserAsync_WithNonexistentUserId_ShouldReturnEmptyList()
	{
		// Arrange
		var cursor = Substitute.For<IAsyncCursor<Article>>();
		cursor.MoveNextAsync().Returns(false);

		_collection.FindAsync(
				Arg.Any<FilterDefinition<Article>>(),
				Arg.Any<FindOptions<Article>>(),
				Arg.Any<CancellationToken>()
		).Returns(cursor);

		// Act
		var result = await _sut.GetByUserAsync("nonexistent");

		// Assert
		result.Should().NotBeNull();
		result.Should().BeEmpty();
	}

	[Fact]
	public async Task FindAsync_ShouldReturnMatchingArticles()
	{
		// Arrange
		var matchingArticles = _testArticles.Where(a => a.Title.Contains("Test")).ToList();
		var cursor = Substitute.For<IAsyncCursor<Article>>();
		cursor.MoveNextAsync().Returns(true, false);
		cursor.Current.Returns(matchingArticles);

		_collection.FindAsync(
				Arg.Any<FilterDefinition<Article>>(),
				Arg.Any<FindOptions<Article>>(),
				Arg.Any<CancellationToken>()
		).Returns(cursor);

		// Act
		var result = await _sut.FindAsync(a => a.Title.Contains("Test"));

		// Assert
		result.Should().BeEquivalentTo(matchingArticles);
	}

	[Fact]
	public async Task GetAllAsync_ShouldReturnAllArticles()
	{
		// Arrange
		var cursor = Substitute.For<IAsyncCursor<Article>>();
		cursor.MoveNextAsync().Returns(true, false);
		cursor.Current.Returns(_testArticles);

		_collection.FindAsync(
				Arg.Any<FilterDefinition<Article>>(),
				Arg.Any<FindOptions<Article>>(),
				Arg.Any<CancellationToken>()
		).Returns(cursor);

		// Act
		var result = await _sut.GetAllAsync();

		// Assert
		result.Should().BeEquivalentTo(_testArticles);
	}

}