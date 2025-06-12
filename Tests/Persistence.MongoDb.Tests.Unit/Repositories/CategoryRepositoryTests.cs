// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CategoryRepositoryTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Persistence.MongoDb.Tests.Unit
// =======================================================

using MongoDB.Bson;

namespace Persistence.Repositories;

/// <summary>
///   Unit tests for the <see cref="CategoryRepository"/> class.
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(CategoryRepository))]
public sealed class CategoryRepositoryTests
{

	private readonly Mock<IAsyncCursor<Category>> _cursor;

	private readonly Mock<IMongoCollection<Category>> _mockCollection;

	private readonly Mock<IMongoDbContextFactory> _mockContext;

	private List<Category> _list = new();

	public CategoryRepositoryTests()
	{
		_cursor = TestFixtures.GetMockCursor(_list);

		_mockCollection = TestFixtures.GetMockCollection(_cursor);

		_mockContext = TestFixtures.GetMockContext();
	}

	private CategoryRepository CreateRepository()
	{
		return new CategoryRepository(_mockContext.Object);
	}

	[Fact(DisplayName = "Archive Category - Success Path")]
	public async Task ArchiveAsync_WithValidCategory_ShouldArchiveTheCategory()
	{

		// Arrange
		var categoryToArchive = FakeCategory.GetNewCategory(true);

		categoryToArchive.Archived = true;

		SetupMongoCollection(categoryToArchive);

		var sut = CreateRepository();

		// Act
		await sut.ArchiveAsync(categoryToArchive);

		// Assert
		_mockCollection.Verify(c => c.ReplaceOneAsync(
				It.IsAny<FilterDefinition<Category>>(),
				It.IsAny<Category>(),
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
		
	}

	[Fact(DisplayName = "Archive Category - Not Found")]
	public async Task ArchiveAsync_WithNonExistentCategory_ShouldReturnFailResult()
	{

		// Arrange
		var categoryToArchive = FakeCategory.GetNewCategory(true);
		SetupMongoCollection(categoryToArchive);

		var replaceResult = new ReplaceOneResult.Acknowledged(0, 0, BsonValue.Create(categoryToArchive.Id));

		_mockCollection.Setup(c => c.ReplaceOneAsync(
						It.IsAny<FilterDefinition<Category>>(),
						It.IsAny<Category>(),
						It.IsAny<ReplaceOptions>(),
						It.IsAny<CancellationToken>()))
				.ReturnsAsync(replaceResult);

		var sut = CreateRepository();

		// Act
		var result = await sut.ArchiveAsync(categoryToArchive);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeFalse();
		result.Error.Should().Contain($"ID {categoryToArchive.Id}");
		
	}

	[Fact(DisplayName = "Archive Category - Exception")]
	public async Task ArchiveAsync_WhenExceptionOccurs_ShouldReturnFailResult()
	{

		// Arrange
		var categoryToArchive = FakeCategory.GetNewCategory(true);
		SetupMongoCollection(categoryToArchive);

		const string errorMessage = "Database connection error";

		_mockCollection.Setup(c => c.ReplaceOneAsync(
						It.IsAny<FilterDefinition<Category>>(),
						It.IsAny<Category>(),
						It.IsAny<ReplaceOptions>(),
						It.IsAny<CancellationToken>()))
				.ThrowsAsync(new Exception(errorMessage));

		var sut = CreateRepository();

		// Act
		var result = await sut.ArchiveAsync(categoryToArchive);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeFalse();
		result.Error.Should().Be(errorMessage);
		
		_mockCollection.Verify(c => c.ReplaceOneAsync(
				It.IsAny<FilterDefinition<Category>>(),
				It.IsAny<Category>(),
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
		
	}

	[Fact(DisplayName = "Create Category - Success Path")]
	public async Task CreateAsync_WithValidCategory_ShouldInsertCategory()
	{

		// Arrange
		var newCategory = FakeCategory.GetNewCategory(true);
		SetupMongoCollection(null);

		var sut = CreateRepository();

		// Act
		var result = await sut.CreateAsync(newCategory);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeTrue();

		_mockCollection.Verify(c => c.InsertOneAsync(
				It.IsAny<Category>(),
				It.IsAny<InsertOneOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
		
	}

	[Fact(DisplayName = "Create Category - Exception")]
	public async Task CreateAsync_WhenExceptionOccurs_ShouldReturnFailResult()
	{

		// Arrange
		var newCategory = FakeCategory.GetNewCategory(true);
		SetupMongoCollection(null);

		const string errorMessage = "Database connection error";

		_mockCollection.Setup(c => c.InsertOneAsync(
						It.IsAny<Category>(),
						It.IsAny<InsertOneOptions>(),
						It.IsAny<CancellationToken>()))
				.ThrowsAsync(new Exception(errorMessage));

		var sut = CreateRepository();

		// Act
		var result = await sut.CreateAsync(newCategory);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeFalse();
		result.Error.Should().Be(errorMessage);
		
	}

	[Fact(DisplayName = "Get Category - Success Path")]
	public async Task GetAsync_WithValidId_ShouldReturnCategory()
	{

		// Arrange
		var expected = FakeCategory.GetNewCategory(true);
		_list = [expected];
		_cursor.Setup(c => c.Current).Returns(_list);
		_mockContext.Setup(c => c.GetCollection<Category>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		var result = await sut.GetAsync(expected.Id);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeTrue();
		result.Value.Should().NotBeNull();
		result.Value.Id.Should().Be(expected.Id);

		_mockCollection.Verify(c => c.FindAsync(
				It.IsAny<FilterDefinition<Category>>(),
				It.IsAny<FindOptions<Category>>(),
				It.IsAny<CancellationToken>()), Times.Once);

	}

	[Fact(DisplayName = "Get Category - Not Found")]
	public async Task GetAsync_WithNonExistentId_ShouldReturnFailResult()
	{

		// Arrange
		var categoryId = ObjectId.GenerateNewId();
		_list = new List<Category>(); // Empty list to simulate category not found
		_cursor.Setup(c => c.Current).Returns(_list);
		_mockContext.Setup(c => c.GetCollection<Category>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		var result = await sut.GetAsync(categoryId);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeFalse();
		result.Error.Should().Contain($"ID {categoryId}");

		_mockCollection.Verify(c => c.FindAsync(
				It.IsAny<FilterDefinition<Category>>(),
				It.IsAny<FindOptions<Category>>(),
				It.IsAny<CancellationToken>()), Times.Once);
		
	}

	[Fact(DisplayName = "Get Category - Exception")]
	public async Task GetAsync_WhenExceptionOccurs_ShouldReturnFailResult()
	{

		// Arrange
		var categoryId = ObjectId.GenerateNewId();
		_mockContext.Setup(c => c.GetCollection<Category>(It.IsAny<string>())).Returns(_mockCollection.Object);

		const string errorMessage = "Database connection error";

		_mockCollection.Setup(c => c.FindAsync(
						It.IsAny<FilterDefinition<Category>>(),
						It.IsAny<FindOptions<Category>>(),
						It.IsAny<CancellationToken>()))
				.ThrowsAsync(new Exception(errorMessage));

		var sut = CreateRepository();

		// Act
		var result = await sut.GetAsync(categoryId);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeFalse();
		result.Error.Should().Be(errorMessage);
				
		_mockCollection.Verify(c => c.FindAsync(
				It.IsAny<FilterDefinition<Category>>(),
				It.IsAny<FindOptions<Category>>(),
				It.IsAny<CancellationToken>()), Times.Once);
		
	}

	[Fact(DisplayName = "Get All Entities - Success Path")]
	public async Task GetAllAsync_ShouldReturnAllEntities()
	{

		// Arrange
		const int expectedCount = 5;

		var entities = FakeCategory.GetCategories(expectedCount, true).ToList();

		_list = new List<Category>(entities);
		_cursor.Setup(c => c.Current).Returns(_list);
		_mockContext.Setup(c => c.GetCollection<Category>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		var result = await sut.GetAllAsync();

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeTrue();
		result.Value.Should().HaveCount(expectedCount);

		_mockCollection.Verify(c => c.FindAsync(
				It.IsAny<FilterDefinition<Category>>(),
				It.IsAny<FindOptions<Category>>(),
				It.IsAny<CancellationToken>()), Times.Once);
		
	}

	[Fact(DisplayName = "Get All Entities - Empty Collection")]
	public async Task GetAllAsync_WithEmptyCollection_ShouldReturnEmptyList()
	{

		// Arrange
		_list = new List<Category>(); // Empty list
		_cursor.Setup(c => c.Current).Returns(_list);
		_mockContext.Setup(c => c.GetCollection<Category>(It.IsAny<string>())).Returns(_mockCollection.Object);

		var sut = CreateRepository();

		// Act
		var result = await sut.GetAllAsync();

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeTrue();
		result.Value.Should().BeEmpty();

		_mockCollection.Verify(c => c.FindAsync(
				It.IsAny<FilterDefinition<Category>>(),
				It.IsAny<FindOptions<Category>>(),
				It.IsAny<CancellationToken>()), Times.Once);
		
	}

	[Fact(DisplayName = "Get All Entities - Exception")]
	public async Task GetAllAsync_WhenExceptionOccurs_ShouldReturnFailResult()
	{

		// Arrange
		_mockContext.Setup(c => c.GetCollection<Category>(It.IsAny<string>())).Returns(_mockCollection.Object);

		const string errorMessage = "Database connection error";

		_mockCollection.Setup(c => c.FindAsync(
						It.IsAny<FilterDefinition<Category>>(),
						It.IsAny<FindOptions<Category>>(),
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
				It.IsAny<FilterDefinition<Category>>(),
				It.IsAny<FindOptions<Category>>(),
				It.IsAny<CancellationToken>()), Times.Once);
		
	}

	[Fact(DisplayName = "Update Category - Success Path")]
	public async Task UpdateAsync_WithValidIdAndCategory_ShouldUpdateCategory()
	{

		// Arrange
		var category = FakeCategory.GetNewCategory(true);
		SetupMongoCollection(category);

		var replaceResult = new ReplaceOneResult.Acknowledged(1, 1, BsonValue.Create(category.Id));

		_mockCollection.Setup(c => c.ReplaceOneAsync(
						It.IsAny<FilterDefinition<Category>>(),
						It.IsAny<Category>(),
						It.IsAny<ReplaceOptions>(),
						It.IsAny<CancellationToken>()))
				.ReturnsAsync(replaceResult);

		var sut = CreateRepository();

		// Act
		var result = await sut.UpdateAsync(category.Id, category);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeTrue();

		_mockCollection.Verify(c => c.ReplaceOneAsync(
				It.IsAny<FilterDefinition<Category>>(),
				It.IsAny<Category>(),
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
		
	}

	[Fact(DisplayName = "Update Category - Not Found")]
	public async Task UpdateAsync_WithNonExistentId_ShouldReturnFailResult()
	{

		// Arrange
		var category = FakeCategory.GetNewCategory(true);
		SetupMongoCollection(category);

		var replaceResult = new ReplaceOneResult.Acknowledged(0, 0, BsonValue.Create(category.Id));

		_mockCollection.Setup(c => c.ReplaceOneAsync(
						It.IsAny<FilterDefinition<Category>>(),
						It.IsAny<Category>(),
						It.IsAny<ReplaceOptions>(),
						It.IsAny<CancellationToken>()))
				.ReturnsAsync(replaceResult);

		var sut = CreateRepository();

		// Act
		var result = await sut.UpdateAsync(category.Id, category);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeFalse();
		result.Error.Should().Contain($"ID {category.Id}");
		
		_mockCollection.Verify(c => c.ReplaceOneAsync(
				It.IsAny<FilterDefinition<Category>>(),
				It.IsAny<Category>(),
				It.IsAny<ReplaceOptions>(),
				It.IsAny<CancellationToken>()), Times.Once);
		
	}

	[Fact(DisplayName = "Update Category - Exception")]
	public async Task UpdateAsync_WhenExceptionOccurs_ShouldReturnFailResult()
	{

		// Arrange
		var category = FakeCategory.GetNewCategory(true);
		SetupMongoCollection(category);

		const string errorMessage = "Database connection error";

		_mockCollection.Setup(c => c.ReplaceOneAsync(
						It.IsAny<FilterDefinition<Category>>(),
						It.IsAny<Category>(),
						It.IsAny<ReplaceOptions>(),
						It.IsAny<CancellationToken>()))
				.ThrowsAsync(new Exception(errorMessage));

		var sut = CreateRepository();

		// Act
		var result = await sut.UpdateAsync(category.Id, category);

		// Assert
		result.Should().NotBeNull();
		result.Success.Should().BeFalse();
		result.Error.Should().Be(errorMessage);

	}

	private void SetupMongoCollection(Category? category)
	{
		if (category is not null)
		{
			_list = [category];
			_cursor.Setup(c => c.Current).Returns(_list);
		}

		_mockContext
				.Setup(c => c.GetCollection<Category>(It.IsAny<string>()))
				.Returns(_mockCollection.Object);
	}

}