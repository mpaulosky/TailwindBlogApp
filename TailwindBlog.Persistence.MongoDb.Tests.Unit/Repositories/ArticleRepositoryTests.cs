// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArticleRepositoryTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb.Tests.Unit
// =======================================================

namespace TailwindBlog.Persistence.Repositories;

[ExcludeFromCodeCoverage]
public class ArticleRepositoryTests
{
	private readonly AppDbContext _dbContext;
	private readonly ArticleRepository _repository;
	private readonly CancellationToken _cancellationToken;

	public ArticleRepositoryTests()
	{
		_dbContext = Substitute.For<AppDbContext>(new DbContextOptions<AppDbContext>());
		_repository = new ArticleRepository(_dbContext);
		_cancellationToken = new CancellationToken();
	}

	[Fact]
	public async Task GetByUserAsync_ShouldReturnArticlesForUser()
	{
		// Arrange
		const string userId = "user123";
		var articles = new List<Article>
		{
			new Article("title1", "intro1", "cover1", "slug1", new AppUserModel { Id = userId }, skipValidation: true),
			new Article("title2", "intro2", "cover2", "slug2", new AppUserModel { Id = userId }, skipValidation: true),
			new Article("title3", "intro3", "cover3", "slug3", new AppUserModel { Id = "other" }, skipValidation: true)
		}.AsQueryable();

		var mockSet = Substitute.For<DbSet<Article>, IAsyncEnumerable<Article>, IQueryable<Article>>();
		SetupMockSet(mockSet, articles);
		_dbContext.Set<Article>().Returns(mockSet);

		// Act
		var result = await _repository.GetByUserAsync(userId);

		// Assert
		result.Should().NotBeNull();
		result.Should().HaveCount(2);
		result.Should().OnlyContain(a => a.Author.Id == userId);
	}

	[Fact]
	public async Task GetByUserAsync_WithNonexistentUserId_ShouldReturnEmptyList()
	{
		// Arrange
		const string userId = "nonexistent";
		var articles = new List<Article>().AsQueryable();

		var mockSet = Substitute.For<DbSet<Article>, IAsyncEnumerable<Article>, IQueryable<Article>>();
		SetupMockSet(mockSet, articles);
		_dbContext.Set<Article>().Returns(mockSet);

		// Act
		var result = await _repository.GetByUserAsync(userId);

		// Assert
		result.Should().NotBeNull();
		result.Should().BeEmpty();
	}

	[Fact]
	public void Add_ShouldAddArticle()
	{
		// Arrange
		var article = new Article(
			"Test Article",
			"Test Introduction",
			"http://test.com/cover.jpg",
			"test-article",
			new AppUserModel { Id = "user1" },
			skipValidation: true
		);
		var mockSet = Substitute.For<DbSet<Article>>();
		_dbContext.Set<Article>().Returns(mockSet);

		// Act
		_repository.Add(article);

		// Assert
		mockSet.Received(1).Add(Arg.Is<Article>(a =>
			a.Title == article.Title &&
			a.Author.Id == article.Author.Id));
	}

	[Fact]
	public void Update_ShouldUpdateArticle()
	{
		// Arrange
		var article = new Article(
			"Test Article",
			"Test Introduction",
			"http://test.com/cover.jpg",
			"test-article",
			new AppUserModel { Id = "user1" },
			skipValidation: true
		);
		var mockSet = Substitute.For<DbSet<Article>>();
		_dbContext.Set<Article>().Returns(mockSet);

		// Act
		_repository.Update(article);

		// Assert
		mockSet.Received(1).Update(Arg.Is<Article>(a =>
			a.Title == article.Title &&
			a.Author.Id == article.Author.Id));
	}

	[Fact]
	public void Remove_ShouldRemoveArticle()
	{
		// Arrange
		var article = new Article(
			"Test Article",
			"Test Introduction",
			"http://test.com/cover.jpg",
			"test-article",
			new AppUserModel { Id = "user1" },
			skipValidation: true
		);
		var mockSet = Substitute.For<DbSet<Article>>();
		_dbContext.Set<Article>().Returns(mockSet);

		// Act
		_repository.Remove(article);

		// Assert
		mockSet.Received(1).Remove(Arg.Is<Article>(a =>
			a.Title == article.Title &&
			a.Author.Id == article.Author.Id));
	}

	[Fact]
	public async Task FindAsync_ShouldReturnMatchingArticles()
	{
		// Arrange
		var articles = new List<Article>
		{
			new Article("Test1", "Intro1", "http://test.com/cover1.jpg", "test1", new AppUserModel { Id = "user1" }, skipValidation: true),
			new Article("Test2", "Intro2", "http://test.com/cover2.jpg", "test2", new AppUserModel { Id = "user1" }, skipValidation: true),
			new Article("Test3", "Intro3", "http://test.com/cover3.jpg", "test3", new AppUserModel { Id = "user2" }, skipValidation: true)
		}.AsQueryable();

		var mockSet = Substitute.For<DbSet<Article>, IAsyncEnumerable<Article>, IQueryable<Article>>();
		SetupMockSet(mockSet, articles);
		_dbContext.Set<Article>().Returns(mockSet);

		// Act
		var result = await _repository.FindAsync(a => a.Title.StartsWith("Test"));

		// Assert
		result.Should().NotBeNull();
		result.Should().HaveCount(3);
		result.Should().OnlyContain(a => a.Title.StartsWith("Test"));
	}

	private void SetupMockSet<T>(DbSet<T> mockSet, IQueryable<T> data) where T : class
	{
		mockSet.As<IAsyncEnumerable<T>>()
			.GetAsyncEnumerator(_cancellationToken)
			.Returns(new TestAsyncEnumerator<T>(data.GetEnumerator()));

		var queryProvider = new TestAsyncQueryProvider<T>(data.Provider);
		mockSet.As<IQueryable<T>>().Provider.Returns(queryProvider);
		mockSet.As<IQueryable<T>>().Expression.Returns(data.Expression);
		mockSet.As<IQueryable<T>>().ElementType.Returns(data.ElementType);
		mockSet.As<IQueryable<T>>().GetEnumerator().Returns(data.GetEnumerator());
	}

	private class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
	{
		private readonly IQueryProvider _inner;

		public TestAsyncQueryProvider(IQueryProvider inner)
		{
			_inner = inner;
		}

		public IQueryable CreateQuery(Expression expression)
		{
			return new TestAsyncEnumerable<TEntity>(expression);
		}

		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
		{
			return new TestAsyncEnumerable<TElement>(expression);
		}

		public object? Execute(Expression expression)
		{
			return _inner.Execute(expression);
		}

		public TResult Execute<TResult>(Expression expression)
		{
			return _inner.Execute<TResult>(expression);
		}

		public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
		{
			return new TestAsyncEnumerable<TResult>(expression);
		}

		public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
		{
			return Execute<TResult>(expression);
		}
	}

	private class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
	{
		public TestAsyncEnumerable(IEnumerable<T> enumerable)
			: base(enumerable)
		{ }

		public TestAsyncEnumerable(Expression expression)
			: base(expression)
		{ }

		public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
		{
			return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
		}

		IQueryProvider IQueryable.Provider
		{
			get { return new TestAsyncQueryProvider<T>(this.AsQueryable().Provider); }
		}
	}

	private class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
	{
		private readonly IEnumerator<T> _inner;

		public TestAsyncEnumerator(IEnumerator<T> inner)
		{
			_inner = inner;
		}

		public void Dispose()
		{
			_inner.Dispose();
		}

		public T Current => _inner.Current;

		public ValueTask<bool> MoveNextAsync()
		{
			return new ValueTask<bool>(_inner.MoveNext());
		}

		public ValueTask DisposeAsync()
		{
			_inner.Dispose();
			return new ValueTask();
		}
	}
}
