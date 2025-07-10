// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CategoryServiceTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Persistence.MongoDb.Tests.Unit
// =======================================================

namespace Persistence.Services;

/// <summary>
/// Unit tests for <see cref="CategoryService"/>.
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(CategoryService))]
public class CategoryServiceTests
{

	private const string _cacheName = "CategoryData";

	private readonly Mock<ICacheService> _cacheMock;

	private readonly Mock<ICategoryRepository> _repositoryMock;

	private readonly CategoryService _sut;

	/// <summary>
	/// Initializes a new instance of the <see cref="CategoryServiceTests"/> class.
	/// </summary>
	public CategoryServiceTests()
	{

		_cacheMock = new Mock<ICacheService>(MockBehavior.Loose);
		_repositoryMock = new Mock<ICategoryRepository>(MockBehavior.Loose);
		_sut = new CategoryService(_repositoryMock.Object, _cacheMock.Object);

	}

	/// <summary>
	/// Ensures constructor throws <see cref="ArgumentNullException"/> when repository is null.
	/// </summary>
	[Fact]
	public void Constructor_NullRepository_ThrowsArgumentNullException()
	{

		// Arrange
		var act = () =>
		{
			var categoryService = new CategoryService(null!, _cacheMock.Object);
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
			var categoryService = new CategoryService(_repositoryMock.Object, null!);
		};

		// Assert
		act.Should().Throw<ArgumentNullException>();

	}

	/// <summary>
	/// Ensures ArchiveAsync returns failure when the category is null.
	/// </summary>
	[Fact]
	public async Task ArchiveAsync_NullCategory_ReturnsFailure()
	{

		// Act
		var result = await _sut.ArchiveAsync(null);

		// Assert
		result.Failure.Should().BeTrue();
		result.Error.Should().Be("Category cannot be null.");

	}

	/// <summary>
	/// Ensures ArchiveAsync returns failure when the repository throws.
	/// </summary>
	[Fact]
	public async Task ArchiveAsync_RepositoryThrows_ReturnsFailure()
	{
		// Arrange
		var dto = new CategoryDto();
		_repositoryMock.Setup(r => r.ArchiveAsync(It.IsAny<Category>())).ThrowsAsync(new Exception("some error"));
		_cacheMock.Setup(c => c.RemoveAsync(_cacheName));

		// Act
		var result = await _sut.ArchiveAsync(dto);

		// Assert
		result.Failure.Should().BeTrue();
		result.Error.Should().Contain("Failed to archive category: some error");
		_cacheMock.Verify(c => c.RemoveAsync(It.IsAny<string>()), Times.Once);
	}

	/// <summary>
	/// Ensures ArchiveAsync returns success when the repository succeeds.
	/// </summary>
	[Fact]
	public async Task ArchiveAsync_Success_ReturnsOk()
	{
		// Arrange
		var dto = new CategoryDto();
		_repositoryMock.Setup(r => r.ArchiveAsync(It.IsAny<Category>())).ReturnsAsync(Result.Ok());
		_cacheMock.Setup(c => c.RemoveAsync(_cacheName));

		// Act
		var result = await _sut.ArchiveAsync(dto);

		// Assert
		result.Success.Should().BeTrue();
		_cacheMock.Verify(c => c.RemoveAsync(_cacheName), Times.Once);
	}

	/// <summary>
	/// Ensures CreateAsync returns failure when the category is null.
	/// </summary>
	[Fact]
	public async Task CreateAsync_NullCategory_ReturnsFailure()
	{
		// Act
		var result = await _sut.CreateAsync(null);

		// Assert
		result.Failure.Should().BeTrue();
		result.Error.Should().Be("Category cannot be null.");
	}

	/// <summary>
	/// Ensures CreateAsync returns failure when the repository throws.
	/// </summary>
	[Fact]
	public async Task CreateAsync_RepositoryThrows_ReturnsFailure()
	{
		// Arrange
		var dto = new CategoryDto();
		_repositoryMock.Setup(r => r.CreateAsync(It.IsAny<Category>())).ThrowsAsync(new Exception("fail!"));
		_cacheMock.Setup(c => c.RemoveAsync(_cacheName));

		// Act
		var result = await _sut.CreateAsync(dto);

		// Assert
		result.Failure.Should().BeTrue();
		result.Error.Should().Contain("Failed to create category: fail!");
		_cacheMock.Verify(c => c.RemoveAsync(_cacheName), Times.Once);
	}

	/// <summary>
	/// Ensures CreateAsync returns success when the repository succeeds.
	/// </summary>
	[Fact]
	public async Task CreateAsync_Success_ReturnsOk()
	{
		// Arrange
		var dto = new CategoryDto();
		_repositoryMock.Setup(r => r.CreateAsync(It.IsAny<Category>())).ReturnsAsync(Result.Ok());
		_cacheMock.Setup(c => c.RemoveAsync(_cacheName));

		// Act
		var result = await _sut.CreateAsync(dto);

		// Assert
		result.Success.Should().BeTrue();
		_cacheMock.Verify(c => c.RemoveAsync(_cacheName), Times.Once);
	}

	/// <summary>
	/// Ensures GetAsync returns failure when category id is empty.
	/// </summary>
	[Fact]
	public async Task GetAsync_CategoryIdEmpty_ReturnsFailure()
	{
		// Act
		var result = await _sut.GetAsync(ObjectId.Empty);

		// Assert
		result.Failure.Should().BeTrue();
		result.Error.Should().Be("Category id cannot be empty.");
	}

	/// <summary>
	/// Ensures GetAsync returns from cache if present.
	/// </summary>
	[Fact]
	public async Task GetAsync_FoundInCache_ReturnsOk()
	{
		// Arrange
		var cachedList = FakeCategoryDto.GetCategoriesDto(3, true);
		var expectedDto = cachedList[0];
		var catId = expectedDto.Id;

		// Create a list of category DTOs to be returned from the cache
		_cacheMock.Setup(x => x.GetAsync<List<CategoryDto>>(_cacheName)).ReturnsAsync(cachedList);

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
		_cacheMock.Setup(c => c.GetAsync<List<CategoryDto>>(_cacheName)).ReturnsAsync((List<CategoryDto>?)null);
		_repositoryMock.Setup(r => r.GetAsync(catId)).ReturnsAsync(Result<Category>.Fail("not found"));

		// Act
		var result = await _sut.GetAsync(catId);

		// Assert
		result.Failure.Should().BeTrue();
		result.Error.Should().Be("Category not found.");
	}

	/// <summary>
	/// Ensures GetAsync returns mapped dto when not in cache and repository returns.
	/// </summary>
	[Fact]
	public async Task GetAsync_NotInCacheAndRepositoryReturns_ReturnsMappedDto()
	{
		// Arrange
		var domainCat = FakeCategory.GetNewCategory(true);
		var catId = domainCat.Id;
		_cacheMock.Setup(c => c.GetAsync<List<CategoryDto>>(_cacheName)).ReturnsAsync((List<CategoryDto>?)null);
		_repositoryMock.Setup(r => r.GetAsync(catId)).ReturnsAsync(Result<Category>.Ok(domainCat));

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
		var cachedCategories = FakeCategory.GetCategories(5, true).Adapt<List<CategoryDto>>();
		_cacheMock.Setup(c => c.GetAsync<List<CategoryDto>>(It.IsAny<string>())).ReturnsAsync(cachedCategories);

		// Act
		var result = await _sut.GetAllAsync();

		// Assert
		result.Success.Should().BeTrue();
		result.Value.Should().NotBeNull();
		_cacheMock.Verify(c => c.GetAsync<List<CategoryDto>>(It.IsAny<string>()));
	}

	/// <summary>
	/// Ensures GetAllAsync returns failure when the cache and repository return failure.
	/// </summary>
	[Fact]
	public async Task GetAllAsync_ValuesNotInCache_AndRepositoryReturnsEmpty_ReturnsFailure()
	{

		// Arrange
		_cacheMock.Setup(c => c.GetAsync<List<CategoryDto>>(_cacheName)).ReturnsAsync((List<CategoryDto>?)null);

		_repositoryMock.Setup(r => r.GetAllAsync())
				.ReturnsAsync(Result<IEnumerable<Category>>.Fail("Failed to retrieve categories."));

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
		var repoList = FakeCategory.GetCategories(5, true);
		_cacheMock.Setup(c => c.GetAsync<List<CategoryDto>>(_cacheName)).ReturnsAsync((List<CategoryDto>?)null);

		_repositoryMock.Setup(r => r.GetAllAsync())
				.ReturnsAsync(Result<IEnumerable<Category>>.Ok(repoList));

		_cacheMock.Setup(c => c.SetAsync(_cacheName, It.IsAny<List<CategoryDto>>(), It.IsAny<TimeSpan>()));

		// Act
		var result = await _sut.GetAllAsync();

		// Assert
		result.Success.Should().BeTrue();
		result.Value.Should().NotBeNull();
	}

	/// <summary>
	/// Ensures UpdateAsync returns failure when the category is null.
	/// </summary>
	[Fact]
	public async Task UpdateAsync_NullCategory_ReturnsFailure()
	{
		// Act
		var result = await _sut.UpdateAsync(null);

		// Assert
		result.Failure.Should().BeTrue();
		result.Error.Should().Be("Category cannot be null");
	}

	/// <summary>
	///   Ensures UpdateAsync returns failure when the repository fails.
	/// </summary>
	[Fact]
	public async Task UpdateAsync_ShouldReturnFail_WhenRepositoryFails()
	{
		// Arrange
		var dto = FakeCategoryDto.GetNewCategoryDto(true);

		_repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<ObjectId>(), It.IsAny<Category>()))
				.ReturnsAsync(Result.Fail("Failed to update category"));

		_cacheMock.Setup(c => c.RemoveAsync(_cacheName));

		// Act
		var result = await _sut.UpdateAsync(dto);

		// Assert
		result.Success.Should().BeFalse();
		result.Error.Should().Be("Failed to update category");
	}

	/// <summary>
	/// Ensures UpdateAsync returns success when the update succeeds.
	/// </summary>
	[Fact]
	public async Task UpdateAsync_Success_ReturnsOk()
	{
		// Arrange
		var category = FakeCategoryDto.GetNewCategoryDto(true);

		_repositoryMock.Setup(r => r.UpdateAsync(category.Id, It.IsAny<Category>()))
				.ReturnsAsync(Result.Ok());

		_cacheMock.Setup(c => c.RemoveAsync(_cacheName));

		// Act
		var result = await _sut.UpdateAsync(category);

		// Assert
		result.Success.Should().BeTrue();
		_cacheMock.Verify(c => c.RemoveAsync(_cacheName), Times.Once);
		_repositoryMock.Verify(r => r.UpdateAsync(category.Id, It.IsAny<Category>()), Times.Once);

	}

}