// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CategoryRepositoryTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb.Tests.Unit
// =======================================================

using TailwindBlog.Persistence.Repositories.TestData;

namespace TailwindBlog.Persistence.Repositories;

[ExcludeFromCodeCoverage]
public sealed class CategoryRepositoryTests
{

	private readonly CategoryRepository _sut;

	private readonly IMongoDatabase _database;

	private readonly IMongoCollection<Category> _collection;

	private readonly Category _testCategory;

	private readonly List<Category> _testCategories;

	public CategoryRepositoryTests()
	{
		_database = Substitute.For<IMongoDatabase>();
		_collection = Substitute.For<IMongoCollection<Category>>();
		_database.GetCollection<Category>("categories").Returns(_collection);

		_sut = new CategoryRepository(_database);

		_testCategory = CategoryTestDataBuilder.CreateDefault();
		_testCategories = CategoryTestDataBuilder.CreateDefaultMany(3);
	}

	[Fact]
	public void Add_ShouldInsertOneCategory()
	{
		// Act
		_sut.Add(_testCategory);

		// Assert
		_collection.Received(1).InsertOne(
				Arg.Is<Category>(c => c.Equals(_testCategory)),
				Arg.Any<InsertOneOptions>());
	}

	[Fact]
	public async Task GetById_ShouldReturnCategory()
	{
		// Arrange
		var cursor = Substitute.For<IAsyncCursor<Category>>();
		cursor.MoveNextAsync().Returns(true, false);
		cursor.Current.Returns([ _testCategory ]);

		_collection.FindAsync(
						Arg.Any<FilterDefinition<Category>>(),
						Arg.Any<FindOptions<Category>>(),
						CancellationToken.None)
				.Returns(cursor);

		// Act
		var result = await _sut.GetByIdAsync(_testCategory.Id);

		// Assert
		result.Should().BeEquivalentTo(_testCategory);
	}

	[Fact]
	public async Task GetById_WhenCategoryNotFound_ShouldReturnNull()
	{
		// Arrange
		var cursor = Substitute.For<IAsyncCursor<Category>>();
		cursor.MoveNextAsync().Returns(false);

		_collection.FindAsync(
						Arg.Any<FilterDefinition<Category>>(),
						Arg.Any<FindOptions<Category>>(),
						CancellationToken.None)
				.Returns(cursor);

		// Act
		var result = await _sut.GetByIdAsync(ObjectId.GenerateNewId());

		// Assert
		result.Should().BeNull();
	}

	[Fact]
	public void Update_ShouldUpdateCategory()
	{
		// Act
		_sut.Update(_testCategory);

		// Assert
		_collection.Received(1).ReplaceOne(
				Arg.Any<FilterDefinition<Category>>(),
				Arg.Is<Category>(c => c.Equals(_testCategory)),
				Arg.Any<ReplaceOptions>());
	}

	[Fact]
	public void Remove_ShouldDeleteCategory()
	{
		// Act
		_sut.Remove(_testCategory);

		// Assert
		_collection.Received(1).DeleteOne(
				Arg.Any<FilterDefinition<Category>>(),
				Arg.Any<CancellationToken>());
	}

	[Fact]
	public async Task GetAll_ShouldReturnAllCategories()
	{
		// Arrange
		var cursor = Substitute.For<IAsyncCursor<Category>>();
		cursor.MoveNextAsync().Returns(true, false);
		cursor.Current.Returns(_testCategories);

		_collection.FindAsync(
						Arg.Any<FilterDefinition<Category>>(),
						Arg.Any<FindOptions<Category>>(),
						CancellationToken.None)
				.Returns(cursor);

		// Act
		var result = await _sut.GetAllAsync();

		// Assert
		result.Should().BeEquivalentTo(_testCategories);
	}

	[Fact]
	public async Task GetAll_WhenNoCategories_ShouldReturnEmptyList()
	{
		// Arrange
		var cursor = Substitute.For<IAsyncCursor<Category>>();
		cursor.MoveNextAsync().Returns(false);

		_collection.FindAsync(
						Arg.Any<FilterDefinition<Category>>(),
						Arg.Any<FindOptions<Category>>(),
						CancellationToken.None)
				.Returns(cursor);

		// Act
		var result = await _sut.GetAllAsync();

		// Assert
		result.Should().BeEmpty();
	}

	[Fact]
	public async Task FindAsync_ShouldReturnMatchingCategories()
	{
		// Arrange
		var matchingCategories = _testCategories.Where(c => c.Name.Contains("Test")).ToList();
		var cursor = Substitute.For<IAsyncCursor<Category>>();
		cursor.MoveNextAsync().Returns(true, false);
		cursor.Current.Returns(matchingCategories);

		_collection.FindAsync(
						Arg.Any<FilterDefinition<Category>>(),
						Arg.Any<FindOptions<Category>>(),
						CancellationToken.None)
				.Returns(cursor);

		// Act
		var result = await _sut.FindAsync(c => c.Name.Contains("Test"));

		// Assert
		result.Should().BeEquivalentTo(matchingCategories);
	}

}