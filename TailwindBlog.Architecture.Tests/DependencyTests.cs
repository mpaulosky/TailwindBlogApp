// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name:     DependencyTests.cs
// Project Name:  TailwindBlog.Architecture.Tests
// =======================================================

namespace TailwindBlog.Architecture.Tests;

public class DependencyTests
{
	[Fact]
	public void MyMediator_Handlers_Should_BeInCorrectNamespace()
	{
		// Arrange
		var assembly = AssemblyReference.ApiService;

		// Act
		var result = Types
				.InAssembly(assembly)
				.That()
				.ImplementInterface(typeof(IRequestHandler<,>))
				.Should()
				.ResideInNamespaceStartingWith("TailwindBlog.ApiService.Features")
				.GetResult();

		// Assert
		result.IsSuccessful.Should().BeTrue();
	}

	[Fact]
	public void Domain_Types_Should_Not_DependOn_Other_Projects()
	{
		// Arrange
		var assembly = AssemblyReference.Domain;

		// Act
		var otherProjects = new[]
		{
						"TailwindBlog.ApiService",
						"TailwindBlog.Web",
						"TailwindBlog.Infrastructure"
				};

		var result = Types
				.InAssembly(assembly)
				.ShouldNot()
				.HaveDependencyOnAny(otherProjects)
				.GetResult();

		// Assert
		result.IsSuccessful.Should().BeTrue();
	}

	[Fact]
	public void Features_Should_NotDependOn_Other_Features()
	{
		// Arrange
		var assembly = AssemblyReference.ApiService;

		// Act
		var result = Types
				.InAssembly(assembly)
				.That()
				.ResideInNamespace("TailwindBlog.ApiService.Features")
				.Should()
				.NotHaveDependencyOnAny("TailwindBlog.ApiService.Features")
				.GetResult();

		// Assert
		result.IsSuccessful.Should().BeTrue();
	}

	[Fact]
	public void Handlers_Should_Have_DependencyOnDomain()
	{
		// Arrange
		var assembly = AssemblyReference.ApiService;

		// Act
		var result = Types
				.InAssembly(assembly)
				.That()
				.ImplementInterface(typeof(IRequestHandler<,>))
				.Should()
				.HaveDependencyOn("TailwindBlog.Domain")
				.GetResult();

		// Assert
		result.IsSuccessful.Should().BeTrue();
	}
}