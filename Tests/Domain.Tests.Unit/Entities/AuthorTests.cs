// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     AuthorTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain.Tests.Unit
// =======================================================

namespace Domain.Entities;

/// <summary>
///   Unit tests for the <see cref="Author" /> entity.
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(Author))]
public class AuthorTests
{

	[Fact]
	public void AppUser_WhenCreated_ShouldHaveCorrectProperties()
	{

		// Arrange & Act
		var user = new Author(Guid.NewGuid().ToString(), "TestUser", "test@example.com", ["Admin"]);

		// Assert
		user.UserName.Should().Be("TestUser");
		user.UserName.Should().Be("TestUser");
		user.Email.Should().Be("test@example.com");
		user.Roles.Should().ContainSingle().Which.Should().Be("Admin");

	}

	[Fact]
	public void AppUser_Empty_ShouldReturnEmptyInstance()
	{

		// Arrange & Act
		var user = Author.Empty;

		// Assert
		user.Id.Should().BeEmpty();
		user.UserName.Should().BeEmpty();
		user.Email.Should().BeEmpty();
		user.Roles.Should().BeEmpty();

	}

	[Fact]
	public void AppUser_Update_ShouldUpdateProperties()
	{

		// Arrange
		var user = new Author(Guid.NewGuid().ToString(), "OldName", "old@example.com", ["User"]);

		// Act
		user.Update("NewName", "new@example.com");

		// Assert
		user.UserName.Should().Be("NewName");
		user.Email.Should().Be("new@example.com");

	}

	[Fact]
	public void AppUser_UpdateRoles_ShouldUpdateRoles()
	{

		// Arrange
		var user = new Author(Guid.NewGuid().ToString(), "User", "user@example.com", ["User"]);

		// Act
		user.UpdateRoles(["Admin", "Editor"]);

		// Assert
		user.Roles.Should().BeEquivalentTo( "Admin", "Editor");

	}

	[Theory]
	[InlineData("", "user@example.com", "Username is required")]
	[InlineData("Us", "user@example.com", "Username must be between 3 and 50 characters")]
	[InlineData("User", "", "Email is required")]
	[InlineData("User", "not-an-email", "Invalid email address format")]
	public void AppUser_WhenCreated_ShouldValidateRequiredFields(string userName, string email, string expectedError)
	{

		// Arrange & Act & Assert
		FluentActions.Invoking(() => new Author(Guid.NewGuid().ToString(), userName, email, ["User"]))
				.Should().Throw<ValidationException>()
				.WithMessage($"*{expectedError}*");

	}

	[Fact]
	public void AppUser_WhenRolesNull_ShouldThrowValidationException()
	{

		// Arrange, Act & Assert
		FluentActions.Invoking(() => new Author(Guid.NewGuid().ToString(), "User", "user@example.com", null!))
				.Should().Throw<ValidationException>()
				.WithMessage("*Roles collection cannot be null*");

	}

}