// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     DependencyInjectionTests.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb.Tests.Unit
// =======================================================

namespace TailwindBlog.Persistence;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(DependencyInjection))]
public class DependencyInjectionTests
{
	[Fact]
	public void AddPersistence_WithValidConfiguration_ShouldRegisterServices()
	{
		// Arrange
		var services = new ServiceCollection();
		var configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(new Dictionary<string, string?>
			{
				{ "ConnectionStrings:blog-app-database", "mongodb://localhost:27017" }
			})
			.Build();

		// Act
		services.AddPersistence(configuration);
		var serviceProvider = services.BuildServiceProvider();

		// Assert
		serviceProvider.GetService<IMongoDatabase>().Should().NotBeNull();
		serviceProvider.GetService<IApplicationDbContext>().Should().NotBeNull();
		serviceProvider.GetService<IArticleRepository>().Should().NotBeNull();
		serviceProvider.GetService<ICategoryRepository>().Should().NotBeNull();
	}

	[Fact]
	public void AddPersistence_WithMissingConnectionString_ShouldThrowException()
	{
		// Arrange
		var services = new ServiceCollection();
		var configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(new Dictionary<string, string?>())
			.Build();

		// Act
		var act = () => services.AddPersistence(configuration);

		// Assert
		act.Should().Throw<InvalidOperationException>()
			.WithMessage("Connection string 'blog-app-database' not found.");
	}
}