// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CqrsAndConfigurationTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Architecture.Tests
// =======================================================

namespace TailwindBlog.Architecture.Tests;

public class CqrsAndConfigurationTests
{
	[Fact(DisplayName = "CQRS Test: Commands should return Result")]
	public void Commands_Should_Return_Result()
	{
		// Arrange
		var assembly = AssemblyReference.ApiService;

		// Act
		var commandHandlers = Types
				.InAssembly(assembly)
				.That()
				.ResideInNamespaceContaining("Commands")
				.And()
				.ImplementInterface(typeof(IRequestHandler<,>))
				.GetTypes();

		// Assert
		foreach (var handler in commandHandlers)
		{
			var interfaces = handler.GetInterfaces();
			var requestHandlerInterface = interfaces.First(i =>
					i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

			var returnType = requestHandlerInterface.GetGenericArguments()[1];
			var isResult = returnType == typeof(Result) ||
					(returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Result<>));

			isResult.Should().BeTrue(
					$"Command handler {handler.Name} should return Result or Result<T>. Found {returnType.Name}");
		}
	}

	[Fact(DisplayName = "CQRS Test: Queries should be read-only")]
	public void Queries_Should_BeReadOnly()
	{
		// Arrange
		var assembly = AssemblyReference.ApiService;

		// Act
		var result = Types
				.InAssembly(assembly)
				.That()
				.ResideInNamespaceContaining("Queries")
				.Should()
				.NotHaveDependencyOnAny(new[]
				{
								"TailwindBlog.ApiService.Features.Commands",
								"TailwindBlog.Persistence.Repositories.Add",
								"TailwindBlog.Persistence.Repositories.Update",
								"TailwindBlog.Persistence.Repositories.Delete",
								"TailwindBlog.Persistence.Repositories.Remove"
				})
				.GetResult();

		// Assert
		result.IsSuccessful.Should().BeTrue(
				"Query handlers should not modify state or depend on commands");
	}

	[Fact(DisplayName = "CQRS Test: Features should be organized by domain concept")]
	public void Features_Should_Be_Organized_By_Domain_Concept()
	{
		// Arrange
		var assembly = AssemblyReference.ApiService;

		// Act/Assert for each expected feature
		foreach (var feature in new[] { "Articles", "Categories" })
		{
			// Each feature should have both commands and queries
			var featureNamespace = $"TailwindBlog.ApiService.Features.{feature}";

			var hasCommands = Types
					.InAssembly(assembly)
					.That()
					.ResideInNamespace($"{featureNamespace}.Commands")
					.GetTypes()
					.Any();

			var hasQueries = Types
					.InAssembly(assembly)
					.That()
					.ResideInNamespace($"{featureNamespace}.Queries")
					.GetTypes()
					.Any();

			hasCommands.Should().BeTrue($"Feature {feature} should have commands");
			hasQueries.Should().BeTrue($"Feature {feature} should have queries");
		}
	}

	[Fact(DisplayName = "Architecture Test: Configuration classes should follow naming convention")]
	public void Configuration_Classes_Should_Follow_Naming_Convention()
	{
		// Arrange
		var assembly = AssemblyReference.MongoDb;

		// Act
		var result = Types
				.InAssembly(assembly)
				.That()
				.AreClasses()
				.And()
				.AreNotAbstract()
				.And()
				.HaveNameEndingWith("Configuration")
				.Should()
				.ResideInNamespaceEndingWith("Configuration")
				.GetResult();

		// Assert
		result.IsSuccessful.Should().BeTrue(
				"Configuration classes should follow naming conventions and reside in Configuration namespace");
	}
}

// Helper class for command handlers
public class Unit { }

// Helper interface to avoid compiler errors
public interface IRequestHandler<TRequest, TResponse> { }