// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     RepositoryTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb.Tests.Unit
// =======================================================

namespace TailwindBlog.Persistence.Repositories;

// [ExcludeFromCodeCoverage]
// [TestSubject(typeof(Repository<>))]
public class RepositoryTests
{
	public class TestEntity : Entity { }

	private readonly AppDbContext _DbContext;
	private readonly TestRepository _Repository;

	public RepositoryTests()
	{
		_DbContext = Substitute.For<AppDbContext>(new DbContextOptions<AppDbContext>());
		_Repository = new TestRepository(_DbContext);
	}

	[Fact]
	public void Add_Should_Add_Entity_To_Context()
	{
		// Arrange
		var entity = new TestEntity();
		var mockSet = Substitute.For<DbSet<TestEntity>>();
		_DbContext.Set<TestEntity>().Returns(mockSet);

		// Act
		_Repository.Add(entity);

		// Assert
		mockSet.Received(1).Add(Arg.Is(entity));
	}

	public class TestRepository : Repository<TestEntity>
	{
		public TestRepository(AppDbContext context) : base(context) { }
	}
}
