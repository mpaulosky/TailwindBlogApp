// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     AppUserDtoTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain.Tests.Unit
// =======================================================

namespace Domain.Models;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(AppUserDto))]
public class AppUserDtoTests
{

	[Fact]
	public void Constructor_Should_Set_Properties()
	{

		// Arrange
		var roles = new List<string> { "Admin" };

		// Act
		var dto = new AppUserDto("id", "user", "email@test.com", roles);

		// Assert
		dto.Id.Should().Be("id");
		dto.UserName.Should().Be("user");
		dto.Email.Should().Be("email@test.com");
		dto.Roles.Should().BeEquivalentTo(roles);

	}

	[Fact]
	public void Default_Constructor_Should_Set_Defaults()
	{

		// Arrange
		// Act
		var dto = new AppUserDto();

		// Assert
		dto.Id.Should().BeEmpty();
		dto.UserName.Should().BeEmpty();
		dto.Email.Should().BeEmpty();
		dto.Roles.Should().BeEmpty();

	}

	[Fact]
	public void Empty_Should_Return_EmptyDto()
	{
		// Arrange & Act
		var emptyDto = AppUserDto.Empty;

		// Assert
		emptyDto.Id.Should().BeEmpty();
		emptyDto.UserName.Should().BeEmpty();
		emptyDto.Email.Should().BeEmpty();
		emptyDto.Roles.Should().BeEmpty();
	}

	[Fact]
	public void Update_Should_Update_UserNameAndEmail()
	{
		// Arrange
		var dto = new AppUserDto("id", "oldUsername", "old@example.com", ["User"], true);
		var newUsername = "newUsername";
		var newEmail = "new@example.com";

		// Act
		dto.Update(newUsername, newEmail);

		// Assert
		dto.UserName.Should().Be(newUsername);
		dto.Email.Should().Be(newEmail);
		dto.Id.Should().Be("id"); // Unchanged
		dto.Roles.Should().BeEquivalentTo(["User"]); // Unchanged
	}

	[Fact]
	public void UpdateRoles_Should_Update_Roles()
	{

		// Arrange
		var dto = FakeAppUserDto.GetNewAppUserDto(true);
		var newRoles = new List<string> { "Admin", "Moderator" };

		// Act
		dto.UpdateRoles(newRoles);

		// Assert
		dto.Roles.Should().BeEquivalentTo(newRoles);

	}

	[Fact]
	public void Constructor_Should_Validate_WhenSkipValidationIsFalse()
	{
		// Arrange
		var invalidId = string.Empty;
		var invalidUsername = "u"; // Too short
		var invalidEmail = "not-an-email";
		var roles = new List<string> { "Admin" };

		// Act & Assert
		var exception =
				Assert.Throws<ValidationException>(() => new AppUserDto(invalidId, invalidUsername, invalidEmail, roles));

		exception.Message.Should().Contain("User ID is required");
	}

	[Theory]
	[InlineData("validUsername", "", "Email is required")] // Invalid email
	[InlineData("", "valid@example.com", "UserName is required")] // Invalid username
	public void Update_Should_Validate_Input(string userName, string email, string message)
	{

		// Arrange
		var dto = FakeAppUserDto.GetNewAppUserDto(true);

		// Act & Assert
		var exception = Assert.Throws<ValidationException>(() => dto.Update(userName, email));

		exception.Message.Should().Contain(message);

	}

	[Theory]
	[InlineData(null, "Roles cannot be null.")] // Null roles
	[InlineData("", "Roles cannot be an empty collection.")] // Empty roles
	public void UpdateRoles_Should_Validate_Input(string? role, string message)
	{

		// Arrange
		var roles = role == null ? null : new List<string>() ;
		var dto = FakeAppUserDto.GetNewAppUserDto(true);

		// Act & Assert
		var exception = Assert.Throws<ValidationException>(() => dto.UpdateRoles(roles!));

		exception.Message.Should().Contain(message);

	}

}