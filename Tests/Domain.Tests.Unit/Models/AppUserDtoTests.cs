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

}