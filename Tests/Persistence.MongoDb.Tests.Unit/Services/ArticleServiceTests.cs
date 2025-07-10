// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArticleServiceTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Persistence.MongoDb.Tests.Unit
// =======================================================

namespace Persistence.Services;

/// <summary>
///   Unit tests for <see cref="ArticleService" />.
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(ArticleService))]
public class ArticleServiceTests
{

	private const string _cacheName = "ArticleData";

	private readonly Mock<ICacheService> _cacheMock;

	private readonly Mock<IArticleRepository> _repositoryMock;

	private readonly ArticleService _sut;

	/// <summary>
	/// Initializes a new instance of the <see cref="ArticleServiceTests"/> class.
	/// </summary>
	public ArticleServiceTests()
	{

		_cacheMock = new Mock<ICacheService>(MockBehavior.Loose);
		_repositoryMock = new Mock<IArticleRepository>(MockBehavior.Loose);
		_sut = new ArticleService(_repositoryMock.Object, _cacheMock.Object);
		
	}

	/// <summary>
	/// Ensures constructor throws <see cref="ArgumentNullException"/> when repository is null.
	/// </summary>
	[Fact]
	public void Constructor_NullRepository_ThrowsArgumentNullException()
	{

		// Arrange
		Action act = () =>
		{
			var articleService = new ArticleService(null!, _cacheMock.Object);
		};

		// Assert
		act.Should().Throw<ArgumentNullException>();
		
	}

	/// <summary>
	/// Ensures constructor throws <see cref="ArgumentNullException"/> when cache is null.
	/// </summary>
	[Fact]
	public void Constructor_NullCache_ThrowsArgumentNullException()
	{

		// Arrange
		var act = () =>
		{
			var articleService = new ArticleService(null!, _cacheMock.Object);
		};

		// Assert
		act.Should().Throw<ArgumentNullException>();
		
	}

	/// <summary>
	/// Ensures ArchiveAsync returns failure when the article is null.
	/// </summary>
	[Fact]
	public async Task ArchiveAsync_NullArticle_ReturnsFailure()
	{

		// Act
		var result = await _sut.ArchiveAsync(null);

		// Assert
		result.Failure.Should().BeTrue();
		result.Error.Should().Be("Article cannot be null.");
		
	}

	/// <summary>
	/// Ensures ArchiveAsync returns failure when the repository throws.
	/// </summary>
	[Fact]
	public async Task ArchiveAsync_RepositoryThrows_ReturnsFailure()
	{
		// Arrange
		var dto = new ArticleDto();
		_repositoryMock.Setup(r => r.ArchiveAsync(It.IsAny<Article>())).ThrowsAsync(new Exception("some error"));
		_cacheMock.Setup(c => c.RemoveAsync(_cacheName));

		// Act
		var result = await _sut.ArchiveAsync(dto);

		// Assert
		result.Failure.Should().BeTrue();
		result.Error.Should().Contain("Failed to archive article: some error");
		_cacheMock.Verify(c => c.RemoveAsync(It.IsAny<string>()), Times.Once);
	}

	/// <summary>
	/// Ensures ArchiveAsync returns success when the repository succeeds.
	/// </summary>
	[Fact]
	public async Task ArchiveAsync_Success_ReturnsOk()
	{
		// Arrange
		var dto = new ArticleDto();
		_repositoryMock.Setup(r => r.ArchiveAsync(It.IsAny<Article>())).ReturnsAsync(Result.Ok());
		_cacheMock.Setup(c => c.RemoveAsync(_cacheName));

		// Act
		var result = await _sut.ArchiveAsync(dto);

		// Assert
		result.Success.Should().BeTrue();
		_cacheMock.Verify(c => c.RemoveAsync(_cacheName), Times.Once);
	}

	/// <summary>
	/// Ensures CreateAsync returns failure when the article is null.
	/// </summary>
	[Fact]
	public async Task CreateAsync_NullArticle_ReturnsFailure()
	{
		// Act
		var result = await _sut.CreateAsync(null);

		// Assert
		result.Failure.Should().BeTrue();
		result.Error.Should().Be("Article cannot be null.");
	}

	/// <summary>
	/// Ensures CreateAsync returns failure when the repository throws.
	/// </summary>
	[Fact]
	public async Task CreateAsync_RepositoryThrows_ReturnsFailure()
	{
		// Arrange
		var dto = new ArticleDto();
		_repositoryMock.Setup(r => r.CreateAsync(It.IsAny<Article>())).ThrowsAsync(new Exception("fail!"));
		_cacheMock.Setup(c => c.RemoveAsync(_cacheName));

		// Act
		var result = await _sut.CreateAsync(dto);

		// Assert
		result.Failure.Should().BeTrue();
		result.Error.Should().Contain("Failed to create article: fail!");
		_cacheMock.Verify(c => c.RemoveAsync(_cacheName), Times.Once);
	}

	/// <summary>
	/// Ensures CreateAsync returns success when the repository succeeds.
	/// </summary>
	[Fact]
	public async Task CreateAsync_Success_ReturnsOk()
	{
		// Arrange
		var dto = new ArticleDto();
		_repositoryMock.Setup(r => r.CreateAsync(It.IsAny<Article>())).ReturnsAsync(Result.Ok());
		_cacheMock.Setup(c => c.RemoveAsync(_cacheName));

		// Act
		var result = await _sut.CreateAsync(dto);

		// Assert
		result.Success.Should().BeTrue();
		_cacheMock.Verify(c => c.RemoveAsync(_cacheName), Times.Once);
	}

	/// <summary>
	/// Ensures GetAsync returns failure when article id is empty.
	/// </summary>
	[Fact]
	public async Task GetAsync_ArticleIdEmpty_ReturnsFailure()
	{
		// Act
		var result = await _sut.GetAsync(ObjectId.Empty);

		// Assert
		result.Failure.Should().BeTrue();
		result.Error.Should().Be("Article id cannot be empty.");
	}

	/// <summary>
	/// Ensures GetAsync returns from cache if present.
	/// </summary>
	[Fact]
	public async Task GetAsync_FoundInCache_ReturnsOk()
	{
		// Arrange
		var cachedList = FakeArticleDto.GetArticleDtos(3, true);
		var expectedDto = cachedList[0];
		var catId = expectedDto.Id;

		// Create a list of article DTOs to be returned from the cache
		_cacheMock.Setup(x => x.GetAsync<List<ArticleDto>>(_cacheName)).ReturnsAsync(cachedList);

		// Act
		var result = await _sut.GetAsync(catId);

		// Assert
		result.Success.Should().BeTrue();
		result.Value.Should().NotBeNull();
		result.Value!.Id.Should().Be(catId);
	}

	/// <summary>
	/// Ensures GetAsync returns failure when not in cache and repository fails.
	/// </summary>
	[Fact]
	public async Task GetAsync_NotInCacheAndRepositoryFails_ReturnsFailure()
	{
		// Arrange
		var catId = ObjectId.GenerateNewId();
		_cacheMock.Setup(c => c.GetAsync<List<ArticleDto>>(_cacheName)).ReturnsAsync((List<ArticleDto>?)null);
		_repositoryMock.Setup(r => r.GetAsync(catId)).ReturnsAsync(Result<Article>.Fail("not found"));

		// Act
		var result = await _sut.GetAsync(catId);

		// Assert
		result.Failure.Should().BeTrue();
		result.Error.Should().Be("Article not found.");
	}

	/// <summary>
	/// Ensures GetAsync returns mapped dto when not in cache and repository returns.
	/// </summary>
	[Fact]
	public async Task GetAsync_NotInCacheAndRepositoryReturns_ReturnsMappedDto()
	{
		// Arrange
		var domainCat = FakeArticle.GetNewArticle(true);
		var catId = domainCat.Id;
		_cacheMock.Setup(c => c.GetAsync<List<ArticleDto>>(_cacheName)).ReturnsAsync((List<ArticleDto>?)null);
		_repositoryMock.Setup(r => r.GetAsync(catId)).ReturnsAsync(Result<Article>.Ok(domainCat));

		// Act
		var result = await _sut.GetAsync(catId);

		// Assert
		result.Success.Should().BeTrue();
		result.Value!.Id.Should().Be(catId);
	}

	/// <summary>
	/// Ensures GetAllAsync returns values from cache if present.
	/// </summary>
	[Fact]
	public async Task GetAllAsync_ValuesInCache_ReturnsFromCache()
	{
		// Arrange
		var cachedCategories = FakeArticle.GetArticles(5, true).Adapt<List<ArticleDto>>();
		_cacheMock.Setup(c => c.GetAsync<List<ArticleDto>>(It.IsAny<string>())).ReturnsAsync(cachedCategories);

		// Act
		var result = await _sut.GetAllAsync();

		// Assert
		result.Success.Should().BeTrue();
		result.Value.Should().NotBeNull();
		_cacheMock.Verify(c => c.GetAsync<List<ArticleDto>>(It.IsAny<string>()));
	}

	/// <summary>
	/// Ensures GetAllAsync returns failure when the repository and cache return failure.
	/// </summary>
	[Fact]
	public async Task GetAllAsync_ValuesNotInCache_AndRepositoryReturnsEmpty_ReturnsFailure()
	{

		// Arrange
		_cacheMock.Setup(c => c.GetAsync<List<ArticleDto>>(_cacheName)).ReturnsAsync((List<ArticleDto>?)null);

		_repositoryMock.Setup(r => r.GetAllAsync())
				.ReturnsAsync(Result<IEnumerable<Article>>.Fail("Failed to retrieve articles."));

		// Act
		var result = await _sut.GetAllAsync();

		// Assert
		result.Failure.Should().BeTrue();
		result.Value.Should().BeNull();

	}

	/// <summary>
	/// Ensures GetAllAsync returns values from the repository and sets cache if not in cache.
	/// </summary>
	[Fact]
	public async Task GetAllAsync_ValuesNotInCache_ReturnsFromRepository_AndSetsCache()
	{
		// Arrange
		var repoList = FakeArticle.GetArticles(5, true);
		_cacheMock.Setup(c => c.GetAsync<List<ArticleDto>>(_cacheName)).ReturnsAsync((List<ArticleDto>?)null);

		_repositoryMock.Setup(r => r.GetAllAsync())
				.ReturnsAsync(Result<IEnumerable<Article>>.Ok(repoList));

		_cacheMock.Setup(c => c.SetAsync(_cacheName, It.IsAny<List<ArticleDto>>(), It.IsAny<TimeSpan>()));

		// Act
		var result = await _sut.GetAllAsync();

		// Assert
		result.Success.Should().BeTrue();
		result.Value.Should().NotBeNull();
	}

	/// <summary>
	///   Ensures GetByUserAsync returns failure when the user is null or id is empty.
	/// </summary>
	[Fact]
	public async Task GetByUserAsync_ShouldReturnFail_WhenUserIsNullOrIdEmpty()
	{
		// Act
		var result1 = await _sut.GetByUserAsync(null);
		var result2 = await _sut.GetByUserAsync(new AppUserDto { Id = null! });

		// Assert
		result1.Success.Should().BeFalse();
		result1.Error.Should().Be("Invalid user.");

		result2.Success.Should().BeFalse();
		result2.Error.Should().Be("Invalid user.");
	}

	/// <summary>
	///   Ensures GetByUserAsync returns failure when GetAllAsync fails.
	/// </summary>
	[Fact]
	public async Task GetByUserAsync_ShouldReturnFail_WhenGetAllFails()
	{
		// arrange
		var user = FakeAppUserDto.GetNewAppUserDto(true);

		_cacheMock.Setup(c => c.GetAsync<List<ArticleDto>>(_cacheName)).ReturnsAsync((List<ArticleDto>?)null);

		_repositoryMock.Setup(r => r.GetAllAsync())
				.ReturnsAsync(Result<IEnumerable<Article>>.Fail("Failed to retrieve articles."));

		// Act
		var result = await _sut.GetByUserAsync(user);

		// Assert
		result.Success.Should().BeFalse();
		result.Error.Should().Be("Could not retrieve articles.");
	}

	/// <summary>
	///   Ensures GetByUserAsync returns only articles for the specified user.
	/// </summary>
	[Fact]
	public async Task GetByUserAsync_ShouldReturnUserArticles()
	{

		// Arrange
		var user = FakeAppUserDto.GetNewAppUserDto(true);
		var repoList = FakeArticle.GetArticles(5, true);
		repoList[0].Author = user; // First article by user

		_cacheMock.Setup(c => c.GetAsync<List<ArticleDto>>(_cacheName)).ReturnsAsync((List<ArticleDto>?)null);

		_repositoryMock.Setup(r => r.GetAllAsync())
				.ReturnsAsync(Result<IEnumerable<Article>>.Ok(repoList));

		// Act
		var result = await _sut.GetByUserAsync(user);

		// Assert
		result.Success.Should().BeTrue();
		result.Value.Should().NotBeNull();
		result.Value.Should().HaveCount(1); // Only one article by this user

	}

	/// <summary>
	/// Ensures UpdateAsync returns failure when the article is null.
	/// </summary>
	[Fact]
	public async Task UpdateAsync_NullArticle_ReturnsFailure()
	{
		// Act
		var result = await _sut.UpdateAsync(null);

		// Assert
		result.Success.Should().BeFalse();
		result.Error.Should().Be("Article cannot be null");
	}

	/// <summary>
	/// Ensures UpdateAsync removes cache and calls repository when the article is valid.
	/// </summary>
	[Fact]
	public async Task UpdateAsync_Success_ReturnsOk()
	{
		// Arrange
		var articleDto = new ArticleDto { Id = ObjectId.GenerateNewId() };
		_repositoryMock.Setup(r => r.UpdateAsync(articleDto.Id, It.IsAny<Article>())).ReturnsAsync(Result.Ok());
		_cacheMock.Setup(c => c.RemoveAsync(_cacheName));

		// Act
		var result = await _sut.UpdateAsync(articleDto);

		// Assert
		result.Success.Should().BeTrue();
		_cacheMock.Verify(c => c.RemoveAsync(It.IsAny<string>()), Times.Once);
		_repositoryMock.Verify(r => r.UpdateAsync(articleDto.Id, It.IsAny<Article>()), Times.Once);
	}

	/// <summary>
	/// Ensures UpdateAsync returns failure when the repository fails.
	/// </summary>
	[Fact]
	public async Task UpdateAsync_ShouldReturnFail_WhenRepositoryFails()
	{
		// Arrange
		var articleDto = new ArticleDto { Id = ObjectId.GenerateNewId() };
		_repositoryMock.Setup(r => r.UpdateAsync(articleDto.Id, It.IsAny<Article>())).ReturnsAsync(Result.Fail("fail"));
		_cacheMock.Setup(c => c.RemoveAsync(_cacheName));

		// Act
		var result = await _sut.UpdateAsync(articleDto);

		// Assert
		result.Success.Should().BeFalse();
		result.Error.Should().Be("Failed to update article");
	}

}