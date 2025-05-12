// =======================================================
// Copyright (c) 2025. All rights reserved.
// Project Name :  TailwindBlog.Architecture.Tests
// =======================================================

using FluentAssertions;
using NetArchTest.Rules;
using Xunit;
using TailwindBlog.ApiService;
using Microsoft.Extensions.DependencyInjection;

namespace TailwindBlog.Architecture.Tests;

public class FeatureTests
{
	[Fact]
	public void Features_Should_BeInCorrectNamespace()
	{
		// Arrange
		var assembly = typeof(Program).Assembly;

		// Act
		var result = Types
				.InAssembly(assembly)
				.That()
				.ResideInNamespaceContaining("Features")
				.Should()
				.ResideInNamespaceEndingWith("Commands")
				.Or()
				.ResideInNamespaceEndingWith("Queries")
				.Or()
				.ResideInNamespaceEndingWith("Models")
				.Or()
				.ResideInNamespaceEndingWith("Validators")
				.GetResult();

		// Assert
		result.IsSuccessful.Should().BeTrue();
	}

	[Fact]
	public void Features_Should_NotReferenceOtherFeatures()
	{
		// Arrange
		var assembly = typeof(Program).Assembly;

		// Act & Assert
		var articleTypes = Types
				.InAssembly(assembly)
				.That()
				.ResideInNamespace("TailwindBlog.ApiService.Features.Article")
				.GetTypes();

		var categoryTypes = Types
				.InAssembly(assembly)
				.That()
				.ResideInNamespace("TailwindBlog.ApiService.Features.Category")
				.GetTypes();

		// Articles should not reference Categories
		foreach (var type in articleTypes)
		{
			type.GetReferencedTypes()
					.Where(t => t.Namespace?.StartsWith("TailwindBlog.ApiService.Features.Category") ?? false)
					.Should()
					.BeEmpty("Article features should not directly reference Category features");
		}

		// Categories should not reference Articles
		foreach (var type in categoryTypes)
		{
			type.GetReferencedTypes()
					.Where(t => t.Namespace?.StartsWith("TailwindBlog.ApiService.Features.Article") ?? false)
					.Should()
					.BeEmpty("Category features should not directly reference Article features");
		}
	}

	[Fact]
	public void Features_Should_BeInternallyConsistent()
	{
		// Arrange
		var assembly = typeof(Program).Assembly;
		var features = new[] { "Article", "Category" };

		foreach (var feature in features)
		{
			// Act
			var featureTypes = Types
					.InAssembly(assembly)
					.That()
					.ResideInNamespace($"TailwindBlog.ApiService.Features.{feature}")
					.GetTypes();

			// Assert
			featureTypes.Should().Contain(t => t.Name.EndsWith("Controller") || t.Name.EndsWith("Endpoints"),
					$"{feature} feature should have a controller or endpoints class");

			var commandNamespace = $"TailwindBlog.ApiService.Features.{feature}.Commands";
			var queryNamespace = $"TailwindBlog.ApiService.Features.{feature}.Queries";

			featureTypes.Where(t => t.Namespace == commandNamespace)
					.Should()
					.Contain(t => t.Name.Contains("Create") || t.Name.Contains("Update") || t.Name.Contains("Delete"),
							$"{feature} feature should have basic CRUD command handlers");

			featureTypes.Where(t => t.Namespace == queryNamespace)
					.Should()
					.Contain(t => t.Name.Contains("Get"),
							$"{feature} feature should have query handlers");
		}
	}

	[Fact]
	public void Feature_Dependencies_Should_BeRegisteredCorrectly()
	{
		// Arrange
		var assembly = typeof(Program).Assembly;

		// Create a new service collection
		var services = new ServiceCollection();
		var startup = new Program();

		// Act - Get the ConfigureServices method through reflection since it's private
		var method = typeof(Program)
				.GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
				.FirstOrDefault(m => m.Name == "ConfigureServices");

		method?.Invoke(startup, new object[] { services });

		// Get all handlers from the Features namespace
		var handlers = Types
				.InAssembly(assembly)
				.That()
				.ResideInNamespaceContaining("Features")
				.And()
				.ImplementInterface(typeof(IRequestHandler<,>))
				.GetTypes();

		// Assert
		foreach (var handler in handlers)
		{
			services.Should().Contain(sd =>
					sd.ServiceType == handler ||
					sd.ServiceType == typeof(IRequestHandler<,>).MakeGenericType(handler.BaseType?.GenericTypeArguments[0], handler.BaseType?.GenericTypeArguments[1]),
					$"Handler {handler.Name} should be registered in DI container");
		}
	}
}
