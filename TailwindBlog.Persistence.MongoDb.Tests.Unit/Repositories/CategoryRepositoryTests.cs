// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CategoryRepositoryTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb.Tests.Unit
// =======================================================


using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using TailwindBlog.Domain.Entities;
using TailwindBlog.Domain.Models;
using TailwindBlog.Persistence.Repositories;
using Testcontainers.MongoDb;
using System.Threading.Tasks;

namespace TailwindBlog.Persistence.MongoDb.Tests.Unit.Repositories;


public class CategoryRepositoryTests : IAsyncLifetime
{
	private readonly MongoDbContainer _mongoDbContainer;
	private AppDbContext _dbContext = null!;
	private CategoryRepository _repository = null!;

	public CategoryRepositoryTests()
	{
		_mongoDbContainer = new MongoDbBuilder()
			.WithImage("mongo:7.0")
			.WithCommand(["--replSet", "rs0"])
			.WithEnvironment("MONGO_INITDB_ROOT_USERNAME", "root")
			.WithEnvironment("MONGO_INITDB_ROOT_PASSWORD", "example")
			.WithCleanUp(true)
			.Build();
	}

	public async Task InitializeAsync()
	{
		await _mongoDbContainer.StartAsync();
		// Initiate the replica set
		var client = new MongoDB.Driver.MongoClient(_mongoDbContainer.GetConnectionString());
		var adminDb = client.GetDatabase("admin");
		var command = new MongoDB.Bson.BsonDocument("replSetInitiate", new MongoDB.Bson.BsonDocument());
		try { await adminDb.RunCommandAsync<MongoDB.Bson.BsonDocument>(command); } catch { /* ignore if already initiated */ }

		var settings = new BlogDatabaseSettings
		{
			ConnectionString = _mongoDbContainer.GetConnectionString(),
			DatabaseName = "test-db"
		};
		_dbContext = new AppDbContext(settings);
		await _dbContext.Database.EnsureCreatedAsync();
		_repository = new CategoryRepository(_dbContext);
	}

	public async Task DisposeAsync()
	{
		await _dbContext.Database.EnsureDeletedAsync();
		await _mongoDbContainer.StopAsync();
		_dbContext.Dispose();
	}

	[Fact]
	public async Task GetAllAsync_Should_Return_All_Categories()
	{
		// Arrange
		var category1 = new Category("Category 1", "Description 1");
		var category2 = new Category("Category 2", "Description 2");
		_dbContext.Categories.AddRange(category1, category2);
		await _dbContext.SaveChangesAsync();

		// Act
		var result = await _repository.GetAllAsync();

		// Assert
		var items = result.ToList();
		items.Should().NotBeNull();
		items.Should().HaveCount(2);
		items.Should().Contain(c => c.Name == "Category 1");
		items.Should().Contain(c => c.Name == "Category 2");
	}

	[Fact]
	public async Task Add_Should_Add_Category_To_Collection()
	{
		// Arrange
		var category = new Category("Test Category", "Test Description");

		// Act
		_repository.Add(category);
		await _dbContext.SaveChangesAsync();

		// Assert
		var dbCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Name == category.Name);
		dbCategory.Should().NotBeNull();
		dbCategory!.Description.Should().Be(category.Description);
	}

	[Fact]
	public async Task GetByIdAsync_Should_Return_Category_When_Found()
	{
		// Arrange
		var category = new Category("Test", "Description");
		_dbContext.Categories.Add(category);
		await _dbContext.SaveChangesAsync();

		// Act
		var result = await _repository.GetByIdAsync(category.Id);

		// Assert
		result.Should().NotBeNull();
		result!.Name.Should().Be("Test");
		result.Description.Should().Be("Description");
	}

	[Fact]
	public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
	{
		// Arrange
		// No categories added

		// Act
		var result = await _repository.GetByIdAsync(Guid.NewGuid());

		// Assert
		result.Should().BeNull();
	}
	// IAsyncLifetime handled by xUnit
}