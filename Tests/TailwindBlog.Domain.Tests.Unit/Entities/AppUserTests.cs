// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     AppUserTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain.Tests.Unit
// =======================================================

namespace TailwindBlog.Domain.Entities;

/// <summary>
///   Unit tests for the <see cref="AppUser" /> entity.
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(AppUser))]
public class AppUserTests
{
	[Fact]
	public void AppUser_WhenCreated_ShouldHaveCorrectProperties()
	{
		// Arrange & Act
		var user = new AppUser(Guid.NewGuid().ToString(), "TestUser", "test@example.com", ["Admin"]);

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
		var user = AppUser.Empty;

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
		var user = new AppUser(Guid.NewGuid().ToString(), "OldName", "old@example.com", ["User"]);

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
		var user = new AppUser(Guid.NewGuid().ToString(), "User", "user@example.com", ["User"]);

		// Act
		user.UpdateRoles(["Admin", "Editor"]);

		// Assert
		user.Roles.Should().BeEquivalentTo(["Admin", "Editor"]);
	}

	[Theory]
	[InlineData("", "user@example.com", "Username is required")]
	[InlineData("Us", "user@example.com", "Username must be between 3 and 50 characters")]
	[InlineData("User", "", "Email is required")]
	[InlineData("User", "not-an-email", "Invalid email address format")]
	public void AppUser_WhenCreated_ShouldValidateRequiredFields(string userName, string email, string expectedError)
	{
		// Arrange & Act & Assert
		FluentActions.Invoking(() => new AppUser(Guid.NewGuid().ToString(), userName, email, ["User"]))
			.Should().Throw<FluentValidation.ValidationException>()
			.WithMessage($"*{expectedError}*");
	}

	[Fact]
	public void AppUser_WhenRolesNull_ShouldThrowValidationException()
	{
		// Arrange, Act & Assert
		FluentActions.Invoking(() => new AppUser(Guid.NewGuid().ToString(), "User", "user@example.com", null!))
			.Should().Throw<FluentValidation.ValidationException>()
			.WithMessage("*Roles collection cannot be null*");
	}
}