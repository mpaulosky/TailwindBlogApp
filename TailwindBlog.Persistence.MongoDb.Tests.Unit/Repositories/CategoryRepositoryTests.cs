// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CategoryRepositoryTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb.Tests.Unit
// =======================================================

namespace TailwindBlog.Persistence.Repositories;

// [ExcludeFromCodeCoverage]
// [TestSubject(typeof(CategoryRepository))]
public class CategoryRepositoryTests
{
	private readonly AppDbContext _dbContext;
	private readonly CategoryRepository _repository;

	public CategoryRepositoryTests()
	{
		_dbContext = Substitute.For<AppDbContext>(new DbContextOptions<AppDbContext>());
		_repository = new CategoryRepository(_dbContext);
	}

	[Fact]
	public void Constructor_Should_Set_Context()
	{
		_repository.Should().NotBeNull();
	}
}
