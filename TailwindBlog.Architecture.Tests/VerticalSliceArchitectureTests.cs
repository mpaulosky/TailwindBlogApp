// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     VerticalSliceArchitectureTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Architecture.Tests
// =======================================================

#region

using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NetArchTest.Rules;
using TailwindBlog.Domain;
using TailwindBlog.Domain.Abstractions;
using TailwindBlog.Domain.Interfaces;
using Xunit;

#endregion

namespace TailwindBlog.Architecture.Tests;

public class VerticalSliceArchitectureTests
{
	[Fact(DisplayName = "Architecture Test: Vertical slices should be isolated")]
	public void Features_Should_Be_Isolated()
	{
		// Arrange
		var assembly = AssemblyReference.ApiService;
		var features = new[] { "Articles", "Categories" };  // Using plural form to match actual namespaces

		// Act & Assert
		foreach (var feature in features)
		{
			var featureNamespace = $"TailwindBlog.ApiService.Features.{feature}";

			// Verify feature structure
			var result = Types
					.InAssembly(assembly)
					.That()
					.ResideInNamespace(featureNamespace)
					.Should()
					.ResideInNamespaceEndingWith("Commands")
					.Or()
					.ResideInNamespaceEndingWith("Queries")
					.Or()
					.ResideInNamespaceEndingWith("Models")
					.Or()
					.ResideInNamespaceEndingWith("Validators")
					.GetResult();

			result.IsSuccessful.Should().BeTrue(
					$"Feature {feature} should follow vertical slice architecture structure");

			// Verify feature isolation
			var otherFeatures = features.Where(f => f != feature);
			foreach (var otherFeature in otherFeatures)
			{
				var dependencyResult = Types
						.InAssembly(assembly)
						.That()
						.ResideInNamespace(featureNamespace)
						.Should()
						.NotHaveDependencyOn($"TailwindBlog.ApiService.Features.{otherFeature}")
						.GetResult();

				dependencyResult.IsSuccessful.Should().BeTrue(
						$"Feature {feature} should not depend on feature {otherFeature}");
			}
		}
	}

	[Fact(DisplayName = "Architecture Test: Verify clean domain layer")]
	public void Domain_Should_Be_Clean()
	{
		var result = Types
				.InAssembly(AssemblyReference.Domain)
				.Should()
				.NotHaveDependencyOnAny(new[]
				{
								"TailwindBlog.ApiService",
								"TailwindBlog.Infrastructure",
								"TailwindBlog.Web"
				})
				.GetResult();

		result.IsSuccessful.Should().BeTrue(
				"Domain layer should not have dependencies on outer layers");
	}
	[Fact(DisplayName = "Architecture Test: Verify interface implementation")]
	public void Interfaces_Should_Follow_DependencyInversion()
	{
		var result = Types
				.InAssembly(AssemblyReference.MongoDb)
				.That()
				.ResideInNamespace("TailwindBlog.Persistence.MongoDb.Repositories")
				.Should()
				.ImplementInterface(typeof(ICategoryRepository))
				.Or()
				.ImplementInterface(typeof(IArticleRepository))
				.GetResult();

		result.IsSuccessful.Should().BeTrue(
				"Repository implementations should implement the domain interfaces");
	}

	[Fact(DisplayName = "Architecture Test: Command handlers should be well encapsulated")]
	public void Command_Handlers_Should_BeWellEncapsulated()
	{
		var assembly = AssemblyReference.ApiService;

		var result = Types
				.InAssembly(assembly)
				.That()
				.ResideInNamespaceContaining("Commands")
				.And()
				.ImplementInterface(typeof(IRequestHandler<,>))
				.Should()
				.BeSealed()
				.And()
				.HaveNameEndingWith("Handler")
				.GetResult();

		result.IsSuccessful.Should().BeTrue(
				"Command handlers should be sealed and follow naming convention");
	}

	[Fact(DisplayName = "Architecture Test: Each feature should have proper dependencies")]
	public void Features_Should_Have_ProperDependencies()
	{
		var assembly = AssemblyReference.ApiService;

		var result = Types
				.InAssembly(assembly)
				.That()
				.ResideInNamespace("TailwindBlog.ApiService.Features")
				.Should()
				.HaveDependencyOn(typeof(Result<>).Namespace!)
				.And()
				.HaveDependencyOn(typeof(IRequestHandler<,>).Namespace!)
				.GetResult();

		result.IsSuccessful.Should().BeTrue(
				"Features should depend on core abstractions and CQRS interfaces");
	}

	[Fact(DisplayName = "Architecture Test: Feature handlers should be registered")]
	public void Feature_Handlers_Should_BeRegistered()
	{
		// Arrange
		var assembly = AssemblyReference.ApiService;
		var services = new ServiceCollection();

		// Add MyMediator
		services.AddMyMediator(assembly);

		// Get all handlers
		var handlers = Types
				.InAssembly(assembly)
				.That()
				.ImplementInterface(typeof(IRequestHandler<,>))
				.GetTypes();

		// Assert
		// There should be at least one handler
		handlers.Should().NotBeEmpty("There should be at least one request handler in the assembly");

		foreach (var handler in handlers)
		{
			// Check that the handler type itself is registered as a service
			var descriptor = services
					.FirstOrDefault(d => d.ImplementationType == handler);

			descriptor.Should().NotBeNull(
					$"Handler {handler.Name} should be registered with dependency injection");

			// Check lifetime - handlers should typically be transient
			descriptor.Lifetime.Should().Be(ServiceLifetime.Transient,
					$"Handler {handler.Name} should have a Transient lifetime");
		}
	}
	[Fact(DisplayName = "Architecture Test: Persistence layer dependencies")]
	public void PersistenceShouldImplementCorrectInterfaces()
	{
		var assembly = AssemblyReference.MongoDb;
		var result = Types
				.InAssembly(assembly)
				.That()
				.ResideInNamespace("TailwindBlog.Persistence.MongoDb.Repositories")
				.Should()
				.ResideInNamespaceStartingWith("TailwindBlog.Persistence.MongoDb")
				.And()
				.ImplementInterface(typeof(IArticleRepository))
				.Or()
				.ImplementInterface(typeof(ICategoryRepository))
				.GetResult();

		result.IsSuccessful.Should().BeTrue(
				"Repository implementations should implement the correct domain interfaces");
	}
}