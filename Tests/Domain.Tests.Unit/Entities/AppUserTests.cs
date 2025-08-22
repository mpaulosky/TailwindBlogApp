// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     AppUserTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain.Tests.Unit
// =======================================================

namespace Domain.Entities;

/// <summary>
///   Unit tests for the <see cref="AppUser" /> entity.
/// </summary>
[ExcludeFromCodeCoverage]
[TestSubject(typeof(AppUser))]
public class AppUserTests
{

	[Fact]
	public void AppUser_ParameterlessConstructor_ShouldSetDefaultValues()
	{
		// Act
		var user = new AppUser();

		// Assert
		user.Id.Should().BeEmpty();
		user.UserName.Should().BeEmpty();
		user.Email.Should().BeEmpty();
		user.Roles.Should().BeEmpty();
	}

	[Fact]
	public void AppUser_Constructor_WithParameters_ShouldSetProperties()
	{
		// Arrange
		var id = "user-1";
		var userName = "TestUser";
		var email = "test@example.com";
		var roles = new List<string> { "Admin", "Editor" };

		// Act
		var user = new AppUser(id, userName, email, roles, true);

		// Assert
		user.Id.Should().Be(id);
		user.UserName.Should().Be(userName);
		user.Email.Should().Be(email);
		user.Roles.Should().BeEquivalentTo(roles);
	}

	[Fact]
	public void AppUser_Constructor_ShouldCallValidateState_WhenSkipValidationIsFalse()
	{
		// Arrange
		var id = "user-1";
		var userName = "TestUser";
		var email = "test@example.com";
		var roles = new List<string> { "Admin" };

		// Act & Assert
		var exception = Record.Exception(() => new AppUser(id, userName, email, roles));
		exception.Should().BeNull();
	}

	[Fact]
	public void AppUser_Constructor_ShouldThrow_WhenInvalidState()
	{
		// Arrange
		var id = "";
		var userName = "";
		var email = "";
		var roles = new List<string>();

		// Act
		var exception = Record.Exception(() => new AppUser(id, userName, email, roles));

		// Assert
		exception.Should().NotBeNull();
	}

}