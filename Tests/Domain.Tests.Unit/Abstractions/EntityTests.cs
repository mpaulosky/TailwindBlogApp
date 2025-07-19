// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     EntityTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain.Tests.Unit
// =======================================================

namespace Domain.Abstractions;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(Entity))]
public class EntityTests
{

	[Fact]
	public void Entity_WhenCreated_ShouldHaveValidId()
	{

		// Arrange & Act
		var entity = new TestEntity();

		// Assert
		entity.Id.Should().NotBe(Guid.Empty);

	}

	[Fact]
	public void Entity_WhenCreated_ShouldHaveCurrentUtcTime()
	{

		// Arrange & Act
		var entity = new TestEntity();

		// Assert
		entity.CreatedOn.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

	}

	[Fact]
	public void Entity_WhenCreated_ShouldHaveNullModifiedOn()
	{

		// Arrange & Act
		var entity = new TestEntity();

		// Assert
		entity.ModifiedOn.Should().BeNull();

	}

	[Fact]
	public void Entity_WhenCreated_ShouldNotBeArchived()
	{

		// Arrange & Act
		var entity = new TestEntity();

		// Assert
		entity.Archived.Should().BeFalse();

	}

	private class TestEntity : Entity { }

}