// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArticleRepositoryTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Persistence.MongoDb.Tests.Unit
// =======================================================

namespace Persistence.Repositories;

/// <summary>
///   Unit tests for the <see cref="ArticleRepository"/> class.
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(ArticleRepository))]
public sealed class ArticleRepositoryTests
{

	private readonly Mock<IAsyncCursor<Article>> _cursor;

	private readonly Mock<IMongoCollection<Article>> _mockCollection;

	private readonly Mock<IMongoDbContextFactory> _mockContext;

	private List<Article> _list = [];

	public ArticleRepositoryTests()
	{
		_cursor = TestFixtures.GetMockCursor(_list);

		_mockCollection = TestFixtures.GetMockCollection(_cursor);

		_mockContext = TestFixtures.GetMockContext();
	}

	private ArticleRepository CreateRepository()
	{
		return new ArticleRepository(_mockContext.Object);
	}

	[Fact(DisplayName = "Archive Article - Success Path")]
	public async Task ArchiveAsync_WithValidArticle_ShouldArchiveTheArticle()
	{

		// Arrange
		var articleToArchive = FakeArticle.GetNewArticle(true);

		articleToArchive.Archived = true;

		SetupMongoCollection(articleToArchive);

		var sut = CreateRepository();

		// Act
		await sut.ArchiveAsync(articleToArchive);

		// Assert
		_mockCollection.Verify(c => c.ReplaceOneAsync(
				It.IsAny<FilterDefinition<Article>>(),
				It.IsAny<Article>(),
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Archive Article - Not Found")]
	public async Task ArchiveAsync_WithNonExistentArticle_ShouldReturnFailResult()
	{

		// Arrange
		var articleToArchive = FakeArticle.GetNewArticle(true);
		SetupMongoCollection(articleToArchive);

		var replaceResult = new ReplaceOneResult.Acknowledged(0, 0, BsonValue.Create(articleToArchive.Id));

		_mockCollection.Setup(c => c.ReplaceOneAsync(
						It.IsAny<FilterDefinition<Article>>(),
						It.IsAny<Article>(),
						It.IsAny<ReplaceOptions>(),
						It.IsAny<CancellationToken>()))
				.ReturnsAsync(replaceResult);

		var sut = CreateRepository();

		// Act
		var result = await sut.ArchiveAsync(articleToArchive);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeFalse();
		result.Error.Should().Contain($"ID {articleToArchive.Id}");

	}

	[Fact(DisplayName = "Archive Article - Exception")]
	public async Task ArchiveAsync_WhenExceptionOccurs_ShouldReturnFailResult()
	{

		// Arrange
		var articleToArchive = FakeArticle.GetNewArticle(true);
		SetupMongoCollection(articleToArchive);

		const string errorMessage = "Database connection error";

		_mockCollection.Setup(c => c.ReplaceOneAsync(
						It.IsAny<FilterDefinition<Article>>(),
						It.IsAny<Article>(),
						It.IsAny<ReplaceOptions>(),
						It.IsAny<CancellationToken>()))
				.ThrowsAsync(new Exception(errorMessage));

		var sut = CreateRepository();

		// Act
		var result = await sut.ArchiveAsync(articleToArchive);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeFalse();
		result.Error.Should().Be(errorMessage);

		_mockCollection.Verify(c => c.ReplaceOneAsync(
				It.IsAny<FilterDefinition<Article>>(),
				It.IsAny<Article>(),
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Create Article - Success Path")]
	public async Task CreateAsync_WithValidArticle_ShouldInsertArticle()
	{

		// Arrange
		var newArticle = FakeArticle.GetNewArticle(true);
		SetupMongoCollection(null);

		var sut = CreateRepository();

		// Act
		var result = await sut.CreateAsync(newArticle);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeTrue();

		_mockCollection.Verify(c => c.InsertOneAsync(
				It.IsAny<Article>(),
				It.IsAny<InsertOneOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Create Article - Exception")]
	public async Task CreateAsync_WhenExceptionOccurs_ShouldReturnFailResult()
	{

		// Arrange
		var newArticle = FakeArticle.GetNewArticle(true);
		SetupMongoCollection(null);

		const string errorMessage = "Database connection error";

		_mockCollection.Setup(c => c.InsertOneAsync(
						It.IsAny<Article>(),
						It.IsAny<InsertOneOptions>(),
						It.IsAny<CancellationToken>()))
				.ThrowsAsync(new Exception(errorMessage));

		var sut = CreateRepository();

		// Act
		var result = await sut.CreateAsync(newArticle);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeFalse();
		result.Error.Should().Be(errorMessage);

	}

	[Fact(DisplayName = "Get Article - Success Path")]
	public async Task GetAsync_WithValidId_ShouldReturnArticle()
	{

		// Arrange
		var expected = FakeArticle.GetNewArticle(true);
		_list = [expected];
		_cursor.Setup(c => c.Current).Returns(_list);
		_mockContext.Setup(c => c.GetCollection<Article>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		var result = await sut.GetAsync(expected.Id);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeTrue();
		result.Value.Should().NotBeNull();
		result.Value.Id.Should().Be(expected.Id);

		_mockCollection.Verify(c => c.FindAsync(
				It.IsAny<FilterDefinition<Article>>(),
				It.IsAny<FindOptions<Article>>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Article - Not Found")]
	public async Task GetAsync_WithNonExistentId_ShouldReturnFailResult()
	{

		// Arrange
		var articleId = ObjectId.GenerateNewId();
		_list = []; // Empty list to simulate article not found
		_cursor.Setup(c => c.Current).Returns(_list);
		_mockContext.Setup(c => c.GetCollection<Article>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		var result = await sut.GetAsync(articleId);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeFalse();
		result.Error.Should().Contain($"ID {articleId}");

		_mockCollection.Verify(c => c.FindAsync(
				It.IsAny<FilterDefinition<Article>>(),
				It.IsAny<FindOptions<Article>>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Article - Exception")]
	public async Task GetAsync_WhenExceptionOccurs_ShouldReturnFailResult()
	{

		// Arrange
		var articleId = ObjectId.GenerateNewId();
		_mockContext.Setup(c => c.GetCollection<Article>(It.IsAny<string>())).Returns(_mockCollection.Object);

		const string errorMessage = "Database connection error";

		_mockCollection.Setup(c => c.FindAsync(
						It.IsAny<FilterDefinition<Article>>(),
						It.IsAny<FindOptions<Article>>(),
						It.IsAny<CancellationToken>()))
				.ThrowsAsync(new Exception(errorMessage));

		var sut = CreateRepository();

		// Act
		var result = await sut.GetAsync(articleId);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeFalse();
		result.Error.Should().Be(errorMessage);

		_mockCollection.Verify(c => c.FindAsync(
				It.IsAny<FilterDefinition<Article>>(),
				It.IsAny<FindOptions<Article>>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get All Entities - Success Path")]
	public async Task GetAllAsync_ShouldReturnAllEntities()
	{

		// Arrange
		const int expectedCount = 5;

		var entities = FakeArticle.GetArticles(expectedCount, true).ToList();

		_list = new List<Article>(entities);
		_cursor.Setup(c => c.Current).Returns(_list);
		_mockContext.Setup(c => c.GetCollection<Article>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		var result = await sut.GetAllAsync();

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeTrue();
		result.Value.Should().HaveCount(expectedCount);

		_mockCollection.Verify(c => c.FindAsync(
				It.IsAny<FilterDefinition<Article>>(),
				It.IsAny<FindOptions<Article>>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get All Entities - Empty Collection")]
	public async Task GetAllAsync_WithEmptyCollection_ShouldReturnEmptyList()
	{

		// Arrange
		_list = []; // Empty list
		_cursor.Setup(c => c.Current).Returns(_list);
		_mockContext.Setup(c => c.GetCollection<Article>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		var result = await sut.GetAllAsync();

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeFalse();
		result.Value.Should().BeNull();

		_mockCollection.Verify(c => c.FindAsync(
				It.IsAny<FilterDefinition<Article>>(),
				It.IsAny<FindOptions<Article>>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get All Entities - Exception")]
	public async Task GetAllAsync_WhenExceptionOccurs_ShouldReturnFailResult()
	{

		// Arrange
		_mockContext.Setup(c => c.GetCollection<Article>(It.IsAny<string>())).Returns(_mockCollection.Object);

		const string errorMessage = "Database connection error";

		_mockCollection.Setup(c => c.FindAsync(
						It.IsAny<FilterDefinition<Article>>(),
						It.IsAny<FindOptions<Article>>(),
						It.IsAny<CancellationToken>()))
				.ThrowsAsync(new Exception(errorMessage));

		var sut = CreateRepository();

		// Act
		var result = await sut.GetAllAsync();

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeFalse();
		result.Error.Should().Be(errorMessage);

		_mockCollection.Verify(c => c.FindAsync(
				It.IsAny<FilterDefinition<Article>>(),
				It.IsAny<FindOptions<Article>>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Update Article - Success Path")]
	public async Task UpdateAsync_WithValidIdAndArticle_ShouldUpdateArticle()
	{

		// Arrange
		var article = FakeArticle.GetNewArticle(true);
		SetupMongoCollection(article);

		var replaceResult = new ReplaceOneResult.Acknowledged(1, 1, BsonValue.Create(article.Id));

		_mockCollection.Setup(c => c.ReplaceOneAsync(
						It.IsAny<FilterDefinition<Article>>(),
						It.IsAny<Article>(),
						It.IsAny<ReplaceOptions>(),
						It.IsAny<CancellationToken>()))
				.ReturnsAsync(replaceResult);

		var sut = CreateRepository();

		// Act
		var result = await sut.UpdateAsync(article.Id, article);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeTrue();

		_mockCollection.Verify(c => c.ReplaceOneAsync(
				It.IsAny<FilterDefinition<Article>>(),
				It.IsAny<Article>(),
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Update Article - Not Found")]
	public async Task UpdateAsync_WithNonExistentId_ShouldReturnFailResult()
	{

		// Arrange
		var article = FakeArticle.GetNewArticle(true);
		SetupMongoCollection(article);

		var replaceResult = new ReplaceOneResult.Acknowledged(0, 0, BsonValue.Create(article.Id));

		_mockCollection.Setup(c => c.ReplaceOneAsync(
						It.IsAny<FilterDefinition<Article>>(),
						It.IsAny<Article>(),
						It.IsAny<ReplaceOptions>(),
						It.IsAny<CancellationToken>()))
				.ReturnsAsync(replaceResult);

		var sut = CreateRepository();

		// Act
		var result = await sut.UpdateAsync(article.Id, article);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeFalse();
		result.Error.Should().Contain($"ID {article.Id}");

		_mockCollection.Verify(c => c.ReplaceOneAsync(
				It.IsAny<FilterDefinition<Article>>(),
				It.IsAny<Article>(),
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Update Article - Exception")]
	public async Task UpdateAsync_WhenExceptionOccurs_ShouldReturnFailResult()
	{

		// Arrange
		var article = FakeArticle.GetNewArticle(true);
		SetupMongoCollection(article);

		const string errorMessage = "Database connection error";

		_mockCollection.Setup(c => c.ReplaceOneAsync(
						It.IsAny<FilterDefinition<Article>>(),
						It.IsAny<Article>(),
						It.IsAny<ReplaceOptions>(),
						It.IsAny<CancellationToken>()))
				.ThrowsAsync(new Exception(errorMessage));

		var sut = CreateRepository();

		// Act
		var result = await sut.UpdateAsync(article.Id, article);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeFalse();
		result.Error.Should().Be(errorMessage);

	}

	[Fact(DisplayName = "Get By User - Success Path")]
	public async Task GetByUserAsync_WithValidUser_ShouldReturnUsersArticles()
	{

		// Arrange
		var entities = FakeArticle.GetArticles(3).ToList();
		var user = entities.First().Author;

		_list = [entities.First()];
		_cursor.Setup(c => c.Current).Returns(_list);
		_mockContext.Setup(c => c.GetCollection<Article>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		var result = await sut.GetByUserAsync(user);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeTrue();
		result.Value.Should().HaveCount(1);

		_mockCollection.Verify(c => c.FindAsync(
				It.IsAny<FilterDefinition<Article>>(),
				It.IsAny<FindOptions<Article>>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get By User - No Articles Found")]
	public async Task GetByUserAsync_WithNoArticles_ShouldReturnFailResult()
	{

		// Arrange
		var user = FakeAppUserDto.GetNewAppUserDto(true);
		_list = []; // Empty list
		_cursor.Setup(c => c.Current).Returns(_list);
		_mockContext.Setup(c => c.GetCollection<Article>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		var result = await sut.GetByUserAsync(user);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeFalse();
		result.Failure.Should().BeTrue();
		result.Error.Should().Contain($"No articles found for user with ID {user.Id}");
		result.Value.Should().BeNull();

		_mockCollection.Verify(c => c.FindAsync(
				It.IsAny<FilterDefinition<Article>>(),
				It.IsAny<FindOptions<Article>>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get By User - Empty User ID")]
	public async Task GetByUserAsync_WithEmptyUserId_ShouldReturnFailResult()
	{

		// Arrange
		var user = FakeAppUserDto.GetNewAppUserDto(true);
		user.Id = string.Empty;
		var entities = FakeArticle.GetArticles(3, true).ToList();

		_list = new List<Article>(entities);
		_cursor.Setup(c => c.Current).Returns(_list);
		_mockContext.Setup(c => c.GetCollection<Article>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		var result = await sut.GetByUserAsync(user);

		// Assert
		result.Success.Should().BeFalse();
		result.Error.Should().Be("Article ID cannot be empty");
		
		_mockCollection.Verify(c => c.FindAsync(
				It.IsAny<FilterDefinition<Article>>(),
				It.IsAny<FindOptions<Article>>(),
				It.IsAny<CancellationToken>()), Times.Never);

	}

	[Fact(DisplayName = "Get By User - Null User")]
	public async Task GetByUserAsync_WhenExceptionOccurs_ShouldReturnFailResult()
	{

		// Arrange
		var user = FakeAppUserDto.GetNewAppUserDto(true);
		var articles = FakeArticle.GetArticles(3, true);

		_list = new List<Article>(articles);
		_cursor.Setup(c => c.Current).Returns(_list);
		const string errorMessage = "Value cannot be null. (Parameter 'collection')";

		_mockCollection.Setup(c => c.FindAsync(
						It.IsAny<FilterDefinition<Article>>(),
						It.IsAny<FindOptions<Article>>(),
						It.IsAny<CancellationToken>()))
				.ThrowsAsync(new Exception(errorMessage));

		var sut = CreateRepository();

		// Act
		var result = await sut.GetByUserAsync(user);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeFalse();
		result.Error.Should().Be(errorMessage);

	}

	private void SetupMongoCollection(Article? article)
	{
		if (article is not null)
		{
			_list = [article];
			_cursor.Setup(c => c.Current).Returns(_list);
		}

		_mockContext
				.Setup(c => c.GetCollection<Article>(It.IsAny<string>()))
				.Returns(_mockCollection.Object);
	}

}