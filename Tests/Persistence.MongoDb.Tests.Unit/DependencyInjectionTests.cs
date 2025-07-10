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

	private const string _databaseName = "articlesdb";

	/// <summary>
	///   Helper to create a configuration with a MongoDB connection string.
	/// </summary>
	private static ConfigurationManager CreateConfiguration(string? connectionString)
	{

		var inMemorySettings = new Dictionary<string, string?>
		{
				{ $"ConnectionStrings:{_databaseName}", connectionString }
		};

		return new ConfigurationBuilder()
				.AddInMemoryCollection(inMemorySettings)
				.Build() as ConfigurationManager ?? throw new InvalidOperationException("Failed to create configuration.");

	}

	[Fact]
	public void AddPersistence_WithValidConnectionString_ShouldRegisterAllDependencies()
	{
		// Arrange
		var services = new ServiceCollection();
		Environment.SetEnvironmentVariable("MongoDb__ConnectionString", _expectedConnectionString);

		try
		{
			// Act
			services.AddPersistence();

			// Assert registrations
			services.Should().ContainSingle(x =>
					x.ServiceType.Name == "IDatabaseSettings" &&
					x.ImplementationInstance != null &&
					x.Lifetime == ServiceLifetime.Singleton
			);

			services.Should().ContainSingle(x =>
					x.ServiceType.Name == "IMongoDbContextFactory" &&
					x.ImplementationType!.Name == "MongoDbContextFactory" &&
					x.Lifetime == ServiceLifetime.Singleton
			);

			services.Should().ContainSingle(x =>
					x.ServiceType.Name == "IArticleRepository" &&
					x.ImplementationType!.Name == "ArticleRepository" &&
					x.Lifetime == ServiceLifetime.Scoped
			);

			services.Should().ContainSingle(x =>
					x.ServiceType.Name == "ICategoryRepository" &&
					x.ImplementationType!.Name == "CategoryRepository" &&
					x.Lifetime == ServiceLifetime.Scoped
			);
		}
		finally
		{
			Environment.SetEnvironmentVariable("MongoDb__ConnectionString", null);
		}
	}

	[Fact]
	public void AddPersistence_WhenConnectionStringIsMissing_ShouldThrowInvalidOperationException()
	{
		// Arrange
		var services = new ServiceCollection();
		Environment.SetEnvironmentVariable("MongoDb__ConnectionString", null);

		// Act
		Action act = () => services.AddPersistence();

		// Assert
		act.Should()
				.Throw<InvalidOperationException>()
				.WithMessage("MongoDB connection string is not configured.");
	}

}