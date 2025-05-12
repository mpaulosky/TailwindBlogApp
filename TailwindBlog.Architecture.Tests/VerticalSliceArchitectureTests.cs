// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     VerticalSliceArchitectureTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Architecture.Tests
// =======================================================

namespace TailwindBlog.Architecture.Tests;

public class VerticalSliceArchitectureTests
{

	[Fact(DisplayName = "Architecture Test: Verify Vertical Slice folder structure")]
	public void FoldersFollowVerticalSliceArchitecture()
	{
		// Act
		var featureTypes = Types
				.InAssembly(AssemblyReference.ApiService)
				.That()
				.ResideInNamespace("TailwindBlog.ApiService.Features")
				.GetTypes();

		var featureStructure = featureTypes
				.Where(t => t.Namespace != null)
				.GroupBy(t => t.Namespace!.Split('.')[4]) // Get feature name (e.g., "Articles", "Categories")
				.ToDictionary(
						g => g.Key,
						g => g.Select(t => t.Namespace).Where(n => n != null).ToList()
				);

		// Assert
		featureStructure.Should().NotBeEmpty("Features should be organized in Features folder");

		foreach (var (featureName, namespaces) in featureStructure)
		{
			var foundRequiredLayers = namespaces.Any(n =>
					n!.Contains($"Features.{featureName}.Commands") ||
					n.Contains($"Features.{featureName}.Queries") ||
					n.Contains($"Features.{featureName}.Models"));

			foundRequiredLayers.Should().BeTrue(
					$"Feature '{featureName}' should have Commands, Queries, and Models folders");
		}
	}

	[Fact(DisplayName = "Architecture Test: Verify feature encapsulation")]
	public void Features_Should_Be_Encapsulated()
	{
		var result = Types
				.InAssembly(AssemblyReference.ApiService)
				.That()
				.ResideInNamespace("TailwindBlog.ApiService.Features")
				.Should()
				.NotHaveDependencyOnAny(
						"TailwindBlog.ApiService.Features.Articles.Commands",
						"TailwindBlog.ApiService.Features.Categories.Commands")
				.GetResult();

		result.IsSuccessful.Should().BeTrue(
				"Features should not have direct dependencies on other features");
	}

	[Fact(DisplayName = "Architecture Test: Verify clean domain layer")]
	public void Domain_Should_Be_Clean()
	{
		var result = Types
				.InAssembly(AssemblyReference.Domain)
				.Should()
				.NotHaveDependencyOnAny(
						"TailwindBlog.ApiService",
						"TailwindBlog.Infrastructure",
						"TailwindBlog.Web")
				.GetResult();

		result.IsSuccessful.Should().BeTrue(
				"Domain layer should not have dependencies on outer layers");
	}

	[Fact(DisplayName = "Architecture Test: Verify interface implementation")]
	public void Interfaces_Should_Follow_DependencyInversion()
	{
		var result = Types
				.InAssembly(AssemblyReference.ApiService)
				.That()
				.ImplementInterface(GenericRepositoryType)
				.Should()
				.ResideInNamespace("TailwindBlog.Persistence.Repositories")
				.GetResult();

		result.IsSuccessful.Should().BeTrue(
				"Repository implementations should be in the correct namespace");
	}

}