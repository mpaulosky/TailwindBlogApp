// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     DependencyInjectionTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : TailwindBlog
// Project Name :  Persistence.MongoDb.Tests.Unit
// =======================================================

namespace Persistence;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(DependencyInjection))]
public class DependencyInjectionTests
{

	private const string _expectedConnectionString = "mongodb://localhost:27017";

	private const string _databaseName = "blog-app-database";

	/// <summary>
	/// Helper to create a configuration with a MongoDB connection string.
	/// </summary>
	private static IConfiguration CreateConfiguration(string? connectionString)
	{

		var inMemorySettings = new Dictionary<string, string?>
		{
				{ $"ConnectionStrings:{_databaseName}", connectionString }
		};

		return new ConfigurationBuilder()
				.AddInMemoryCollection(inMemorySettings)
				.Build();

	}

	[Fact]
	public void AddPersistence_WithValidConnectionString_ShouldRegisterAllDependencies()
	{

		// Arrange
		var services = new ServiceCollection();
		var config = CreateConfiguration(_expectedConnectionString);

		// Act
		services.AddPersistence(config);

		// Assert registrations
		// IDatabaseSettings as singleton
		services.Should().ContainSingle(x =>
				x.ServiceType.Name == "IDatabaseSettings" &&
				x.ImplementationInstance != null &&
				x.Lifetime == ServiceLifetime.Singleton
		);

		// IMongoDbContextFactory as a singleton
		services.Should().ContainSingle(x =>
				x.ServiceType.Name == "IMongoDbContextFactory" &&
				x.ImplementationType!.Name == "MongoDbContextFactory" &&
				x.Lifetime == ServiceLifetime.Singleton
		);

		// IArticleRepository as scoped
		services.Should().ContainSingle(x =>
				x.ServiceType.Name == "IArticleRepository" &&
				x.ImplementationType!.Name == "ArticleRepository" &&
				x.Lifetime == ServiceLifetime.Scoped
		);

		// ICategoryRepository as scoped
		services.Should().ContainSingle(x =>
				x.ServiceType.Name == "ICategoryRepository" &&
				x.ImplementationType!.Name == "CategoryRepository" &&
				x.Lifetime == ServiceLifetime.Scoped
		);

	}

	[Fact]
	public void AddPersistence_WhenConnectionStringIsMissing_ShouldThrowInvalidOperationException()
	{

		// Arrange
		var services = new ServiceCollection();
		var config = CreateConfiguration(null);

		// Act
		Action act = () => services.AddPersistence( config);

		// Assert
		act.Should()
				.Throw<InvalidOperationException>()
				.WithMessage($"Connection string '{_databaseName}'*");

	}

}