// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     EnumTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain.Tests.Unit
// =======================================================

namespace TailwindBlog.Domain.Enums;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(Enum))]
public class EnumTests
{

	[Fact]
	public void Roles_ShouldHaveCorrectValues()
	{

		// Assert
		((int)Roles.Author).Should().Be(0);
		((int)Roles.Admin).Should().Be(1);
		((int)Roles.Reader).Should().Be(2);

	}

	[Fact]
	public void CategoryNames_ShouldHaveCorrectValues()
	{

		// Assert
		((int)CategoryNames.AspNetCore).Should().Be(0);
		((int)CategoryNames.BlazorServer).Should().Be(1);
		((int)CategoryNames.BlazorWasm).Should().Be(2);
		((int)CategoryNames.EntityFrameworkCore).Should().Be(3);
		((int)CategoryNames.NetMaui).Should().Be(4);
		((int)CategoryNames.Other).Should().Be(5);

	}

	[Fact]
	public void Roles_ShouldHaveAllExpectedValues()
	{

		// Arrange
		var expectedRoles = new[] { "Author", "Admin", "Reader" };

		// Act
		var actualRoles = Enum.GetNames<Roles>();

		// Assert
		actualRoles.Should().BeEquivalentTo(expectedRoles);

	}

	[Fact]
	public void CategoryNames_ShouldHaveAllExpectedValues()
	{

		// Arrange
		var expectedCategories = new[]
		{
				"AspNetCore",
				"BlazorServer",
				"BlazorWasm",
				"EntityFrameworkCore",
				"NetMaui",
				"Other"
		};

		// Act
		var actualCategories = Enum.GetNames<CategoryNames>();

		// Assert
		actualCategories.Should().BeEquivalentTo(expectedCategories);

	}

}