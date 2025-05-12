// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     DependencyInjectionTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Persistence.MongoDb.Tests.Unit
// =======================================================

namespace TailwindBlog.Persistence;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(DependencyInjection))]
public class DependencyInjectionTests
{
	[Fact]
	public void AddPersistence_Should_Throw_When_ConnectionString_Missing()
	{
		// Arrange
		var services = new ServiceCollection();
		var config = new ConfigurationRoot(new List<IConfigurationProvider> {
			 new MemoryConfigurationProvider(new MemoryConfigurationSource())
		 });

		// Act
		Action act = () => DependencyInjection.AddPersistence(services, config);

		// Assert
		act.Should().Throw<InvalidOperationException>()
			.WithMessage("Connection string 'tailwind-blog' not found.");
	}
}
