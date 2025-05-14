// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     RepositoryTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb.Tests.Unit
// =======================================================

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace TailwindBlog.Persistence.Repositories;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(Repository<>))]

// public class RepositoryTests
// {
// 	public class TestEntity : Entity
// 	{
// 		public string Title { get; set; } = string.Empty;
// 	}
//
// 	private class TestRepository : Repository<TestEntity>
// 	{
// 		public TestRepository(AppDbContext context) : base(context)
// 		{
// 		}
// 	}
//
// 	private readonly AppDbContext _dbContext;
// 	private readonly Repository<TestEntity> _repository;
// 	private readonly CancellationToken _cancellationToken;
//
// 	public RepositoryTests()
// 	{
// 		_dbContext = Substitute.For<AppDbContext>(new DbContextOptions<AppDbContext>());
// 		_repository = new TestRepository(_dbContext);
// 		_cancellationToken = new CancellationToken();
// 	}
//
// 	[Fact]
// 	public void Constructor_Should_Set_Context()
// 	{
// 		// Assert
// 		_repository.Should().NotBeNull();
// 	}
//
// 	[Fact]
// 	public void Add_Should_Add_Entity_To_Context()
// 	{
// 		// Arrange
// 		var entity = new TestEntity { Id = ObjectId.GenerateNewId(), Title = "Test" };
// 		var mockSet = Substitute.For<DbSet<TestEntity>>();
// 		_dbContext.Set<TestEntity>().Returns(mockSet);
//
// 		// Act
// 		_repository.Add(entity);
//
// 		// Assert
// 		mockSet.Received(1).Add(Arg.Is<TestEntity>(e =>
// 				e.Id == entity.Id &&
// 				e.Title == entity.Title));
// 	}
//
// [Fact]
// public async Task GetAllAsync_Should_Return_All_Entities()
// {
// 	// Arrange
// 	var entities = new List<TestEntity>
// 	{
// 		new() { Id = ObjectId.GenerateNewId(), Title = "Test Entity 1" },
// 		new() { Id = ObjectId.GenerateNewId(), Title = "Test Entity 2" }
// 	}.AsQueryable();
//
// 	var mockSet = Substitute.For<DbSet<TestEntity>, IQueryable<TestEntity>>();
// 	SetupMockSet(mockSet, entities);
// 	_dbContext.Set<TestEntity>().Returns(mockSet);
//
// 	// Act
// 	var result = await _repository.GetAllAsync();
//
// 	// Assert
// 	var testEntities = result.ToList();
// 	testEntities.Should().NotBeNull();
// 	testEntities.Should().HaveCount(2);
// 	testEntities[0].Title.Should().Be("Test Entity 1");
// 	testEntities[1].Title.Should().Be("Test Entity 2");
// }
//
// 	[Fact]
// 	public async Task GetByIdAsync_Should_Return_Entity_When_Found()
// 	{
// 		// Arrange
// 		var entity = new TestEntity { Id = ObjectId.GenerateNewId(), Title = "Test" };
// 		var entities = new List<TestEntity> { entity }.AsQueryable();
//
// 		var mockSet = Substitute.For<DbSet<TestEntity>, IQueryable<TestEntity>>();
// 		SetupMockSet(mockSet, entities);
// 		_dbContext.Set<TestEntity>().Returns(mockSet);
//
// 		// Act
// 		var result = await _repository.GetByIdAsync(entity.Id);
//
// 		// Assert
// 		result.Should().NotBeNull();
// 		result!.Id.Should().Be(entity.Id);
// 		result.Title.Should().Be(entity.Title);
// 	}
//
// 	[Fact]
// 	public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
// 	{
// 		// Arrange
// 		var entities = new List<TestEntity>().AsQueryable();
//
// 		var mockSet = Substitute.For<DbSet<TestEntity>, IQueryable<TestEntity>>();
// 		SetupMockSet(mockSet, entities);
// 		_dbContext.Set<TestEntity>().Returns(mockSet);
//
// 		// Act
// 		var result = await _repository.GetByIdAsync(ObjectId.GenerateNewId());
//
// 		// Assert
// 		result.Should().BeNull();
// 	}
//
// 	[Fact]
// 	public async Task FindAsync_Should_Return_Matching_Entities_With_Predicate()
// 	{
// 		// Arrange
// 		var entities = new List<TestEntity>
// 				{
// 						new() { Id = ObjectId.GenerateNewId(), Title = "Test Entity" },
// 						new() { Id = ObjectId.GenerateNewId(), Title = "Other Entity" }
// 				}.AsQueryable();
//
// 		var mockSet = Substitute.For<DbSet<TestEntity>, IQueryable<TestEntity>>();
// 		SetupMockSet(mockSet, entities);
// 		_dbContext.Set<TestEntity>().Returns(mockSet);
//
// 		// Act
// 		var result = await _repository.FindAsync(e => e.Title.StartsWith("Test"));
//
// 		// Assert
// 		var testEntities = result.ToList();
// 		testEntities.Should().NotBeNull();
// 		testEntities.Should().HaveCount(1);
// 		testEntities.First().Title.Should().Be("Test Entity");
// 	}
//
// 	[Fact]
// 	public void Update_Should_Update_Entity_In_Context()
// 	{
// 		// Arrange
// 		var entity = new TestEntity { Id = ObjectId.GenerateNewId(), Title = "Test" };
// 		var mockSet = Substitute.For<DbSet<TestEntity>>();
// 		_dbContext.Set<TestEntity>().Returns(mockSet);
//
// 		// Act
// 		_repository.Update(entity);
//
// 		// Assert
// 		mockSet.Received(1).Update(Arg.Is<TestEntity>(e =>
// 				e.Id == entity.Id &&
// 				e.Title == entity.Title));
// 	}
//
// 	[Fact]
// 	public void Remove_Should_Remove_Entity_From_Context()
// 	{
// 		// Arrange
// 		var entity = new TestEntity { Id = ObjectId.GenerateNewId(), Title = "Test" };
// 		var mockSet = Substitute.For<DbSet<TestEntity>>();
// 		_dbContext.Set<TestEntity>().Returns(mockSet);
//
// 		// Act
// 		_repository.Remove(entity);
//
// 		// Assert
// 		mockSet.Received(1).Remove(Arg.Is<TestEntity>(e =>
// 				e.Id == entity.Id &&
// 				e.Title == entity.Title));
// 	}
// 	private void SetupMockSet<T>(DbSet<T> mockSet, IQueryable<T> data) where T : class
// 	{
// 		var asyncEnumerable = Substitute.For<IAsyncEnumerable<T>>();
// 		asyncEnumerable.GetAsyncEnumerator(Arg.Any<CancellationToken>())
// 				.Returns(new TestAsyncEnumerator<T>(data.GetEnumerator()));
// 		mockSet.As<IAsyncEnumerable<T>>().Returns(asyncEnumerable);
//
// 		var queryProvider = new TestAsyncQueryProvider<T>(data.Provider);
// 		var queryable = Substitute.For<IQueryable<T>>();
// 		queryable.Provider.Returns(queryProvider);
// 		queryable.Expression.Returns(data.Expression);
// 		queryable.ElementType.Returns(data.ElementType);
// 		queryable.GetEnumerator().Returns(data.GetEnumerator());
// 		mockSet.As<IQueryable<T>>().Returns(queryable);
// 	}
//
// 	private class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
// 	{
// 		private readonly IQueryProvider _inner;
//
// 		public TestAsyncQueryProvider(IQueryProvider inner)
// 		{
// 			_inner = inner;
// 		}
//
// 		public IQueryable CreateQuery(Expression expression)
// 		{
// 			return new TestAsyncEnumerable<TEntity>(expression);
// 		}
//
// 		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
// 		{
// 			return new TestAsyncEnumerable<TElement>(expression);
// 		}
//
// 		public object? Execute(Expression expression)
// 		{
// 			return _inner.Execute(expression);
// 		}
//
// 		public TResult Execute<TResult>(Expression expression)
// 		{
// 			return _inner.Execute<TResult>(expression);
// 		}
//
// 		public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
// 		{
// 			return new TestAsyncEnumerable<TResult>(expression);
// 		}
//
// 		public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
// 		{
// 			return Execute<TResult>(expression);
// 		}
// 	}
//
// 	private class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
// 	{
// 		public TestAsyncEnumerable(IEnumerable<T> enumerable)
// 				: base(enumerable)
// 		{ }
//
// 		public TestAsyncEnumerable(Expression expression)
// 				: base(expression)
// 		{ }
//
// 		public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
// 		{
// 			return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
// 		}
//
// 		IQueryProvider IQueryable.Provider
// 		{
// 			get { return new TestAsyncQueryProvider<T>(this.AsQueryable().Provider); }
// 		}
// 	}
//
// 	private class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
// 	{
// 		private readonly IEnumerator<T> _inner;
//
// 		public TestAsyncEnumerator(IEnumerator<T> inner)
// 		{
// 			_inner = inner;
// 		}
//
// 		public T Current => _inner.Current;
//
// 		public ValueTask<bool> MoveNextAsync()
// 		{
// 			return new ValueTask<bool>(_inner.MoveNext());
// 		}
//
// 		public ValueTask DisposeAsync()
// 		{
// 			_inner.Dispose();
// 			return new ValueTask();
// 		}
// 	}
// }
public class RepositoryTests
{

	private readonly IMongoDatabase _mockDatabase;

	private readonly IMongoCollection<TestEntity> _mockCollection;

	private readonly Repository<TestEntity> _sut;

	public RepositoryTests()
	{
		_mockDatabase = Substitute.For<IMongoDatabase>();
		_mockCollection = Substitute.For<IMongoCollection<TestEntity>>();
		_mockDatabase.GetCollection<TestEntity>(typeof(TestEntity).Name).Returns(_mockCollection);
		_sut = Repository<TestEntity>.CreateForTests(_mockDatabase);
	}

	public class TestEntity : Entity
	{

		public string Name { get; set; } = string.Empty;

	}

	[Fact]
	public async Task GetByIdAsync_ShouldReturnEntity_WhenEntityExists()
	{
		// Arrange
		var id = ObjectId.GenerateNewId();
		var expected = new TestEntity { Id = id, Name = "Test" };
		var cursor = Substitute.For<IAsyncCursor<TestEntity>>();
		cursor.MoveNext(default).Returns(true, false);
		cursor.Current.Returns(new[] { expected });

		_mockCollection.Find(Arg.Any<FilterDefinition<TestEntity>>())
				.Returns(cursor);

		// Act
		var result = await _sut.GetByIdAsync(id);

		// Assert
		result.Should().NotBeNull();
		result!.Id.Should().Be(id);
		result.Name.Should().Be("Test");
	}

	[Fact]
	public async Task GetAllAsync_ShouldReturnAllEntities()
	{
		// Arrange
		var entities = new[]
		{
				new TestEntity { Id = "1", Name = "Test1" },
				new TestEntity { Id = "2", Name = "Test2" }
		};

		var cursor = Substitute.For<IAsyncCursor<TestEntity>>();
		cursor.MoveNext(default).Returns(true, false);
		cursor.Current.Returns(entities);

		_mockCollection.Find(Arg.Any<FilterDefinition<TestEntity>>())
				.Returns(cursor);

		// Act
		var result = await _sut.GetAllAsync();

		// Assert
		result.Should().BeEquivalentTo(entities);
	}

	[Fact]
	public async Task CreateAsync_ShouldInsertEntity()
	{
		// Arrange
		var entity = new TestEntity { Id = "1", Name = "Test" };

		// Act
		var result = await _sut.CreateAsync(entity);

		// Assert
		await _mockCollection.Received(1).InsertOneAsync(
				Arg.Is<TestEntity>(e => e.Id == entity.Id && e.Name == entity.Name),
				Arg.Any<InsertOneOptions>(),
				Arg.Any<CancellationToken>()
		);

		result.Should().BeEquivalentTo(entity);
	}

	[Fact]
	public async Task UpdateAsync_ShouldReplaceEntity()
	{
		// Arrange
		var entity = new TestEntity { Id = "1", Name = "Updated" };

		// Act
		await _sut.UpdateAsync(entity);

		// Assert
		await _mockCollection.Received(1).ReplaceOneAsync(
				Arg.Any<FilterDefinition<TestEntity>>(),
				Arg.Is<TestEntity>(e => e.Id == entity.Id && e.Name == entity.Name),
				Arg.Any<ReplaceOptions>(),
				Arg.Any<CancellationToken>()
		);
	}

	[Fact]
	public async Task DeleteAsync_ShouldDeleteEntity()
	{
		// Arrange
		var id = "1";

		// Act
		await _sut.DeleteAsync(id);

		// Assert
		await _mockCollection.Received(1).DeleteOneAsync(
				Arg.Any<FilterDefinition<TestEntity>>(),
				Arg.Any<DeleteOptions>(),
				Arg.Any<CancellationToken>()
		);
	}

}