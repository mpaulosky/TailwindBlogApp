// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     BlogDatabaseSettingsTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain.Tests.Unit
// =======================================================

namespace Domain.Models;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(BlogDatabaseSettings))]
public class BlogDatabaseSettingsTests
{

	[Fact]
	public void Default_Properties_Should_Be_Empty()
	{

		// Arrange & Act
		var settings = new BlogDatabaseSettings();

		// Assert
		settings.ConnectionString.Should().BeEmpty();
		settings.DatabaseName.Should().BeEmpty();

	}

	[Fact]
	public void Properties_Should_Be_Settable()
	{

		// Arrange & Act
		var settings = new BlogDatabaseSettings
		{
				ConnectionString = "conn",
				DatabaseName = "db"
		};

		// Assert
		settings.ConnectionString.Should().Be("conn");
		settings.DatabaseName.Should().Be("db");

	}

}