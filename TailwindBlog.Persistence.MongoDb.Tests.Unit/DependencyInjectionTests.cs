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
	public void AddPersistence_ShouldRegisterRequiredServices()
	{
		// Arrange
		var services = new ServiceCollection();
		var configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(new[]
			{
				new KeyValuePair<string, string?>("ConnectionStrings:tailwind-blog", "mongodb://localhost:27017")
			})
			.Build();

		// Act
		services.AddPersistence(configuration);

		// Assert
		var provider = services.BuildServiceProvider();

		provider.GetService<IApplicationDbContext>().Should().NotBeNull();
		provider.GetService<IUnitOfWork>().Should().NotBeNull();
		provider.GetService<IArticleRepository>().Should().NotBeNull();
		provider.GetService<ICategoryRepository>().Should().NotBeNull();
	}

	[Fact]
	public void AddPersistence_WithMissingConnectionString_ShouldThrowException()
	{
		// Arrange
		var services = new ServiceCollection();
		var configuration = new ConfigurationBuilder().Build();

		// Act
		var act = () => services.AddPersistence(configuration);

		// Assert
		act.Should().Throw<InvalidOperationException>()
			.WithMessage("Connection string 'tailwind-blog' not found.");
	}
}
