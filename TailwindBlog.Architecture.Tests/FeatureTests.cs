// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     FeatureTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Architecture.Tests
// =======================================================

namespace TailwindBlog.Architecture.Tests;

public class FeatureTests
{
	[Fact]
	public void Features_Should_BeInCorrectNamespace()
	{
		// Arrange
		var assembly = AssemblyReference.ApiService;

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
		var assembly = AssemblyReference.ApiService;

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

		articleTypes.Should().NotBeEmpty();
		categoryTypes.Should().NotBeEmpty();

		foreach (var type in articleTypes)
		{
			Types.InAssembly(assembly)
					.That()
					.HaveNameMatching(type.Name)
					.Should()
					.NotHaveDependencyOn("TailwindBlog.ApiService.Features.Category")
					.GetResult()
					.IsSuccessful.Should().BeTrue();
		}

		foreach (var type in categoryTypes)
		{
			Types.InAssembly(assembly)
					.That()
					.HaveNameMatching(type.Name)
					.Should()
					.NotHaveDependencyOn("TailwindBlog.ApiService.Features.Article")
					.GetResult()
					.IsSuccessful.Should().BeTrue();
		}
	}

	[Fact]
	public void Features_Should_OnlyUseValidatedCommands()
	{
		// Arrange
		var assembly = AssemblyReference.ApiService;

		// Act
		var result = Types
				.InAssembly(assembly)
				.That()
				.ResideInNamespaceContaining("Commands")
				.Should()
				.HaveNameEndingWith("Command")
				.And()
				.HaveCustomAttribute(typeof(ValidationAttribute))
				.GetResult();

		// Assert
		result.IsSuccessful.Should().BeTrue();
	}
}