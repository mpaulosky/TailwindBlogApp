// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArchitectureTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Architecture.Tests
// =======================================================

namespace TailwindBlog.Architecture.Tests;

public class ArchitectureTests
{
	private const string _domainNamespace = "TailwindBlog.Domain";
	private const string _applicationNamespace = "TailwindBlog.ApiService";
	private const string _infrastructureNamespace = "TailwindBlog.Persistence";
	private const string _presentationNamespace = "TailwindBlog.Web";
	private const string _sharedKernelNamespace = "TailwindBlog.ServiceDefaults";
	private const string _mediatorNamespace = "MyMediator";

	[Fact(DisplayName = "Architecture Test: Domain should be clean, no dependencies on other layers")]
	public void Domain_Should_BeClean()
	{
		var result = Types
				.InAssembly(AssemblyReference.Domain)
				.Should()
				.NotHaveDependencyOnAny(new[]
				{
								_applicationNamespace,
								_infrastructureNamespace,
								_presentationNamespace
				})
				.GetResult();

		result.IsSuccessful.Should().BeTrue(
				"Domain layer should be independent of other layers");
	}

	[Fact(DisplayName = "Architecture Test: Application layer should not depend on infrastructure/presentation")]
	public void Application_Should_Not_DependOnInfrastructureOrPresentation()
	{
		var result = Types
				.InAssembly(AssemblyReference.ApiService)
				.Should()
				.NotHaveDependencyOnAny(new[]
				{
								_infrastructureNamespace,
								_presentationNamespace
				})
				.GetResult();

		result.IsSuccessful.Should().BeTrue(
				"Application layer should not depend on infrastructure or presentation");
	}

	[Fact(DisplayName = "Architecture Test: Presentation layer should not depend on infrastructure")]
	public void Presentation_Should_Not_DependOnInfrastructure()
	{
		var result = Types
				.InAssembly(AssemblyReference.Web)
				.Should()
				.NotHaveDependencyOn(_infrastructureNamespace)
				.GetResult();

		result.IsSuccessful.Should().BeTrue(
				"Presentation layer should not depend on infrastructure");
	}

	[Fact(DisplayName = "Architecture Test: Projects follow CQRS pattern")]
	public void Application_Should_FollowCqrsPattern()
	{
		// Verify Commands
		var commandsResult = Types
				.InAssembly(AssemblyReference.ApiService)
				.That()
				.ResideInNamespaceContaining("Commands")
				.Should()
				.HaveNameEndingWith("Command")
				.GetResult();

		commandsResult.IsSuccessful.Should().BeTrue(
				"Commands should follow naming convention");

		// Verify Queries
		var queriesResult = Types
				.InAssembly(AssemblyReference.ApiService)
				.That()
				.ResideInNamespaceContaining("Queries")
				.Should()
				.HaveNameEndingWith("Query")
				.GetResult();

		queriesResult.IsSuccessful.Should().BeTrue(
				"Queries should follow naming convention");

		// Verify Handlers
		var handlersResult = Types
				.InAssembly(AssemblyReference.ApiService)
				.That()
				.ImplementInterface(typeof(IRequestHandler<,>))
				.Should()
				.HaveNameEndingWith("Handler")
				.GetResult();

		handlersResult.IsSuccessful.Should().BeTrue(
				"Handlers should follow naming convention");
	}

	[Fact(DisplayName = "Architecture Test: Entities follow proper encapsulation")]
	public void Domain_Entities_Should_BeProperlyEncapsulated()
	{
		var entityTypes = Types
				.InAssembly(AssemblyReference.Domain)
				.That()
				.ResideInNamespace($"{_domainNamespace}.Entities")
				.GetTypes();

		foreach (var entityType in entityTypes)
		{
			// Check that all mutable properties have private setters
			var mutableProperties = entityType.GetProperties()
					.Where(p => p.CanWrite);

			foreach (var prop in mutableProperties)
			{
				var setter = prop.GetSetMethod(nonPublic: true);
				setter.Should().NotBeNull("Properties should have setters");
				setter!.IsPublic.Should().BeFalse(
						$"Property {prop.Name} in {entityType.Name} should have a private setter");
			}
		}
	}

	[Fact(DisplayName = "Architecture Test: Features are organized by vertical slices")]
	public void Application_Should_BeOrganizedByFeatures()
	{
		var featureTypes = Types
				.InAssembly(AssemblyReference.ApiService)
				.That()
				.ResideInNamespace($"{_applicationNamespace}.Features");

		// Verify Command organization
		var commandsResult = featureTypes
				.Should()
				.ResideInNamespaceEndingWith("Commands")
				.GetResult();

		commandsResult.IsSuccessful.Should().BeTrue(
				"Commands should be organized in feature-specific Commands folders");

		// Verify Query organization
		var queriesResult = featureTypes
				.Should()
				.ResideInNamespaceEndingWith("Queries")
				.GetResult();

		queriesResult.IsSuccessful.Should().BeTrue(
				"Queries should be organized in feature-specific Queries folders");
	}

	[Fact(DisplayName = "Architecture Test: Proper use of interfaces")]
	public void Infrastructure_Should_DependOnInterfaces()
	{
		var result = Types
				.InAssembly(AssemblyReference.MongoDb)
				.That()
				.HaveNameEndingWith("Repository")
				.Should()
				.ImplementInterface(typeof(IRepository<>))
				.GetResult();

		result.IsSuccessful.Should().BeTrue(
				"Infrastructure classes should depend on domain interfaces");
	}

	[Fact(DisplayName = "Architecture Test: Proper use of dependency injection")]
	public void Should_UseDependencyInjection()
	{
		var assemblies = new[]
		{
						AssemblyReference.ApiService,
						AssemblyReference.MongoDb,
						AssemblyReference.Web
				};

		foreach (var assembly in assemblies)
		{
			var result = Types
					.InAssembly(assembly)
					.That()
					.HaveNameEndingWith("Service")
					.Or()
					.HaveNameEndingWith("Repository")
					.Or()
					.HaveNameEndingWith("Handler")
					.Should()
					.BePublic()
					.And()
					.HaveDependencyOn("Microsoft.Extensions.DependencyInjection")
					.GetResult();

			result.IsSuccessful.Should().BeTrue(
					$"Classes in {assembly.GetName().Name} should use dependency injection");
		}
	}
}