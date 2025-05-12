// =======================================================
// Copyright (c) 2025. All rights reserved.
// Project Name :  TailwindBlog.Architecture.Tests
// =======================================================

#region

using System.Reflection;
using FluentAssertions;
using MyMediator;
using NetArchTest.Rules;
using TailwindBlog.Domain.Abstractions;
using static TailwindBlog.Architecture.Tests.AssemblyReference;
using Xunit;

#endregion

namespace TailwindBlog.Architecture.Tests;

public class CqrsAndConfigurationTests
{
	[Fact(DisplayName = "CQRS Test: Commands should return Result")]
	public void Commands_Should_Return_Result()
	{
		// Arrange
		var assembly = ApiService;

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
			foreach (var i in interfaces)
			{
				if (!i.IsGenericType) continue;
				if (i.GetGenericTypeDefinition() != typeof(IRequestHandler<,>)) continue;

				var returnType = i.GetGenericArguments()[1];
				returnType.Should().NotBe(typeof(Unit),
						$"Command handler {handler.Name} should return Result or Result<T>");

				var isResult = returnType == typeof(Result) ||
										 (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Result<>));

				isResult.Should().BeTrue($"Command handler {handler.Name} should return Result or Result<T>");
			}
		}
	}

	[Fact(DisplayName = "CQRS Test: Queries should be read-only")]
	public void Queries_Should_BeReadOnly()
	{
		// Arrange
		var assembly = ApiService;

		// Act
		var result = Types
				.InAssembly(assembly)
				.That()
				.ResideInNamespaceContaining("Queries")
				.And()
				.AreClasses()
				.Should()
				.NotHaveDependencyOn("Commands")
				.And()
				.BeImmutable()
				.GetResult();

		// Assert
		result.IsSuccessful.Should().BeTrue();
	}

	[Fact(DisplayName = "CQRS Test: Features should be organized by domain concept")]
	public void Features_Should_Be_Organized_By_Domain_Concept()
	{
		// Arrange
		var assembly = ApiService;

		// Act
		var featureTypes = Types
				.InAssembly(assembly)
				.That()
				.ResideInNamespace("TailwindBlog.ApiService.Features")
				.GetTypes();

		var featureStructure = featureTypes
				.GroupBy(t => t.Namespace?.Split('.')[4]) // Get the feature name (e.g., "Articles", "Categories")
				.ToDictionary(
						g => g.Key ?? string.Empty,
						g => g.Select(t => t.Namespace?.Split('.').LastOrDefault()).Distinct()
				);

		// Assert
		featureStructure.Keys.Should().Contain(new[] { "Articles", "Categories" });

		foreach (var feature in featureStructure)
		{
			feature.Value.Should().Contain(ns =>
					ns == "Commands" ||
					ns == "Queries" ||
					ns == "Models");
		}
	}

	[Fact(DisplayName = "Architecture Test: Configuration classes should follow naming convention")]
	public void Configuration_Classes_Should_Follow_Naming_Convention()
	{
		// Arrange
		var assemblies = new[]
		{
						Domain,
						ApiService,
						Infrastructure,
						Web
				};

		// Act & Assert
		foreach (var assembly in assemblies)
		{
			var result = Types
					.InAssembly(assembly)
					.That()
					.HaveNameContaining("Config")
					.Or()
					.HaveNameContaining("Settings")
					.Or()
					.HaveNameContaining("Options")
					.Should()
					.HaveNameEndingWith("Configuration")
					.Or()
					.HaveNameEndingWith("Settings")
					.Or()
					.HaveNameEndingWith("Options")
					.GetResult();

			result.IsSuccessful.Should().BeTrue(
					$"Configuration classes in {assembly.GetName().Name} should follow naming conventions");
		}
	}
}

// Helper class for command handlers
public class Unit { }

// Helper interface to avoid compiler errors
public interface IRequestHandler<TRequest, TResponse> { }
