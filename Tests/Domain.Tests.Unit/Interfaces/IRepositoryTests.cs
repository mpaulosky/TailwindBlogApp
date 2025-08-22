// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     IRepositoryTests.cs
// Company :       mpaulosky
// Author :        GitHub Copilot
// Solution Name : TailwindBlog
// Project Name :  Domain.Tests.Unit
// =======================================================

using Domain.Abstractions;

namespace Domain.Interfaces;

/// <summary>
///   Unit tests for IRepository interface contract.
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(IRepository<>))]
public class IRepositoryTests
{

	[Fact]
	public void IRepository_ShouldSupportBasicCrudOperations()
	{

		// Arrange
		var repo = Substitute.For<IRepository<string>>();
		var entity = "test-entity";

		// Act
		repo.CreateAsync(entity);
		repo.Received(1).CreateAsync(entity);
		repo.ArchiveAsync(entity);
		repo.Received(1).ArchiveAsync(entity);

	}

	[Fact]
	public async Task IRepository_ShouldSupportFindById()
	{

		// Arrange
		var repo = Substitute.For<IRepository<string>>();
		var id = Guid.NewGuid();
		repo.GetAsync(id).Returns("entity1");

		// Act
		var result = await repo.GetAsync(id);

		// Assert
		result.Should().NotBeNull();
		result.Should().BeOfType<Result<string>>();
		result.Success.Should().BeTrue();
		result.Error.Should().BeNull();
		result.Value.Should().Be("entity1");

	}

}