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
using TailwindBlog.Domain.Interfaces;
using Xunit;

#endregion

namespace TailwindBlog.Architecture.Tests;

public class ArchitectureTests
{
	private const string DomainNamespace = "TailwindBlog.Domain";
	private const string ApplicationNamespace = "TailwindBlog.ApiService";
	private const string InfrastructureNamespace = "TailwindBlog.Persistence";
	private const string PresentationNamespace = "TailwindBlog.Web";
	private const string SharedKernelNamespace = "TailwindBlog.ServiceDefaults";
	private const string MediatorNamespace = "MyMediator";

	[Fact(DisplayName = "Architecture Test: Domain should be clean, no dependencies on other layers")]
	public void Domain_Should_BeClean()
	{
		// Arrange
		var dependencies = new[]
		{
						ApplicationNamespace,
						InfrastructureNamespace,
						PresentationNamespace
				};

		// Act
		var result = dependencies.HasDependencyOnAll(AssemblyReference.Domain);

		// Assert
		result.Should().BeTrue();
	}

	[Fact(DisplayName = "Architecture Test: Application layer should not depend on infrastructure/presentation")]
	public void Application_Should_Not_DependOnInfrastructureOrPresentation()
	{
		// Arrange & Act
		var hasInfrastructureDependency = InfrastructureNamespace.HasDependencyOn(AssemblyReference.ApiService);
		var hasPresentationDependency = PresentationNamespace.HasDependencyOn(AssemblyReference.ApiService);

		// Assert
		hasInfrastructureDependency.Should().BeTrue();
		hasPresentationDependency.Should().BeTrue();
	}

	[Fact(DisplayName = "Architecture Test: Presentation layer should not depend on infrastructure")]
	public void Presentation_Should_Not_DependOnInfrastructure()
	{
		// Arrange & Act
		var result = InfrastructureNamespace.HasDependencyOn(AssemblyReference.Web);

		// Assert
		result.Should().BeTrue();
	}

	[Fact(DisplayName = "Architecture Test: Projects follow CQRS pattern")]
	public void Application_Should_FollowCqrsPattern()
	{
		// Arrange & Act
		var assembly = AssemblyReference.ApiService;

		var hasCommands = Types
				.InAssembly(assembly)
				.That()
				.HaveNameEndingWith("Command")
				.Should()
				.ImplementInterface(typeof(IRequest<>))
				.GetResult()
				.IsSuccessful;

		var hasQueries = Types
				.InAssembly(assembly)
				.That()
				.HaveNameEndingWith("Query")
				.Should()
				.ImplementInterface(typeof(IRequest<>))
				.GetResult()
				.IsSuccessful;

		var hasHandlers = Types
				.InAssembly(assembly)
				.That()
				.HaveNameEndingWith("Handler")
				.Should()
				.ImplementInterface(typeof(IRequestHandler<,>))
				.GetResult()
				.IsSuccessful;

		// Assert
		hasCommands.Should().BeTrue();
		hasQueries.Should().BeTrue();
		hasHandlers.Should().BeTrue();
	}

	[Fact(DisplayName = "Architecture Test: Entities follow proper encapsulation")]
	public void Domain_Entities_Should_BeProperlyEncapsulated()
	{
		// Arrange & Act
		var result = Types
				.InAssembly(AssemblyReference.Domain)
				.That()
				.Inherit(typeof(Entity))
				.Should()
				.BeSealed()
				.Or()
				.BeAbstract()
				.GetResult()
				.IsSuccessful;

		// Assert
		result.Should().BeTrue();
	}

	[Fact(DisplayName = "Architecture Test: Features are organized by vertical slices")]
	public void Application_Should_BeOrganizedByFeatures()
	{
		// Arrange & Act
		var result = Types
				.InAssembly(AssemblyReference.ApiService)
				.That()
				.ResideInNamespace($"{ApplicationNamespace}.Features")
				.Should()
				.ResideInNamespaceEndingWith("Commands")
				.Or()
				.ResideInNamespaceEndingWith("Queries")
				.Or()
				.ResideInNamespaceEndingWith("Models")
				.GetResult()
				.IsSuccessful;

		// Assert
		result.Should().BeTrue();
	}

	[Fact(DisplayName = "Architecture Test: Proper use of interfaces")]
	public void Infrastructure_Should_DependOnInterfaces()
	{
		// Arrange & Act
		var result = Types
				.InAssembly(AssemblyReference.Infrastructure)
				.That()
				.HaveNameEndingWith("Repository")
				.Should()
				.ImplementInterface(typeof(IRepository<>))
				.GetResult()
				.IsSuccessful;

		// Assert
		result.Should().BeTrue();
	}
}
