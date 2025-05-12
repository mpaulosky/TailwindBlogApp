// =======================================================
// Copyright (c) 2025. All rights reserved.
// Project Name :  TailwindBlog.Architecture.Tests
// =======================================================

using System.Reflection;
using FluentAssertions;
using NetArchTest.Rules;
using TailwindBlog.Domain.Abstractions;
using static TailwindBlog.Architecture.Tests.AssemblyReference;
using Xunit;

namespace TailwindBlog.Architecture.Tests;

public class VerticalSliceArchitectureTests
{
	[Fact(DisplayName = "Architecture Test: Verify Vertical Slice folder structure")]
	public void FoldersFollowVerticalSliceArchitecture()
	{
		// Arrange
		var apiAssembly = ApiService;
		var domainAssembly = Domain;

		// Act
		var featureTypes = Types.InAssembly(apiAssembly)
				.That()
				.ResideInNamespace("TailwindBlog.ApiService.Features")
				.GetTypes();

		var featureStructure = featureTypes
				.GroupBy(t => t.Namespace?.Split('.')[4]) // Get the feature name (e.g., "Articles", "Categories")
				.ToDictionary(
						g => g.Key ?? string.Empty,
						g => g.Select(t => t.Namespace).Distinct().ToList()
				);

		// Assert
		// 1. Verify base feature organization
		featureStructure.Should().NotBeEmpty("Features should be organized in the Features folder");

		// 2. Verify each feature has the required components
		foreach (var feature in featureStructure)
		{
			var featureName = feature.Key;
			var namespaces = feature.Value;

			// Each feature should have commands, queries, and models
			namespaces.Should().Contain(n =>
					n?.Contains($"Features.{featureName}.Commands") == true ||
					n?.Contains($"Features.{featureName}.Queries") == true ||
					n?.Contains($"Features.{featureName}.Models") == true,
					$"Feature '{featureName}' should have Commands, Queries, and Models");
		}
	}

	[Fact(DisplayName = "Architecture Test: Verify feature encapsulation")]
	public void Features_Should_Be_Encapsulated()
	{
		// Arrange
		var apiAssembly = ApiService;

		// Act
		var result = Types.InAssembly(apiAssembly)
				.That()
				.ResideInNamespace("TailwindBlog.ApiService.Features")
				.Should()
				.NotHaveDependencyOnAny(
						"TailwindBlog.ApiService.Features.Articles.Commands",
						"TailwindBlog.ApiService.Features.Categories.Commands")
				.GetResult();

		// Assert
		result.IsSuccessful.Should().BeTrue(
				"Features should not have direct dependencies on other features' implementations");
	}

	[Fact(DisplayName = "Architecture Test: Verify clean domain layer")]
	public void Domain_Should_Be_Clean()
	{
		// Arrange & Act
		var domainAssembly = Domain;
		var result = Types.InAssembly(domainAssembly)
				.ShouldNot()
				.HaveDependencyOnAny(
						"TailwindBlog.ApiService",
						"TailwindBlog.Infrastructure",
						"TailwindBlog.Web")
				.GetResult();

		// Assert
		result.IsSuccessful.Should().BeTrue(
				"Domain layer should not have dependencies on outer layers");
	}

	[Fact(DisplayName = "Architecture Test: Verify proper interface abstractions")]
	public void Interfaces_Should_Follow_DependencyInversion()
	{
		// Arrange & Act
		var result = Types.InAssembly(ApiService)
				.That()
				.ImplementInterface(typeof(IRepository<>))
				.Should()
				.ResideInNamespace("TailwindBlog.Persistence.Repositories")
				.And()
				.HaveDependencyOn("TailwindBlog.Domain")
				.GetResult();

		// Assert  
		result.IsSuccessful.Should().BeTrue(
				"Repository implementations should depend on domain interfaces");
	}

	[Fact(DisplayName = "Architecture Test: Verify proper model usage")]
	public void Models_Should_Be_Used_Correctly()
	{
		// Arrange & Act
		var result = Types.InAssembly(ApiService)
				.That()
				.ResideInNamespace("TailwindBlog.ApiService.Features")
				.And()
				.HaveNameEndingWith("Command")
				.Or()
				.HaveNameEndingWith("Query")
				.Should()
				.HaveDependencyOn("TailwindBlog.Domain.Models")
				.GetResult();

		// Assert
		result.IsSuccessful.Should().BeTrue(
				"Commands and Queries should use domain models");
	}
}
