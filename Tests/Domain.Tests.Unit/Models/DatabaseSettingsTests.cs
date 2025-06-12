// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     DatabaseSettingsTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain.Tests.Unit
// =======================================================

namespace Domain.Models;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(DatabaseSettings))]
public class DatabaseSettingsTests
{

	[Fact]
	public void Constructor_Should_Set_Properties()
	{

		// Arrange & Act
		var settings = new DatabaseSettings("conn", "db");

		// Assert
		settings.ConnectionStrings.Should().Be("conn");
		settings.DatabaseName.Should().Be("db");

	}

	[Fact]
	public void Default_Constructor_Should_Allow_Setting_Properties()
	{

		// Arrange & Act
		var settings = new DatabaseSettings
		{
				ConnectionStrings = "abc",
				DatabaseName = "def"
		};

		// Assert
		settings.ConnectionStrings.Should().Be("abc");
		settings.DatabaseName.Should().Be("def");

	}

}