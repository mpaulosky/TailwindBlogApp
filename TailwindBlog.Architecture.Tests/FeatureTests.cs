// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     FeatureTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Architecture.Tests
// =======================================================

using Mono.Cecil;

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

		articleTypes.Should().NotBeEmpty("The Article feature namespace should contain types");
		categoryTypes.Should().NotBeEmpty("The Category feature namespace should contain types");
	
		foreach (var type in articleTypes)
		{
			Types.InAssembly(assembly)
					.That()
					.HaveNameMatching(type.Name)
					.Should()
					.NotHaveDependencyOn("TailwindBlog.ApiService.Features.Category")
					.GetResult()
					.IsSuccessful.Should().BeTrue($"{type.Name} should not depend on Category feature");
		}
	
		foreach (var type in categoryTypes)
		{
			Types.InAssembly(assembly)
					.That()
					.HaveNameMatching(type.Name)
					.Should()
					.NotHaveDependencyOn("TailwindBlog.ApiService.Features.Article")
					.GetResult()
					.IsSuccessful.Should().BeTrue($"{type.Name} should not depend on Article feature");
		}
	}

	[Fact]
	public void Features_Should_OnlyUseValidatedCommands()
	{
		// Arrange
		var assembly = AssemblyReference.ApiService;
		var commandValidationRule = new CommandValidationRule();
	
		// Act
		var result = Types
				.InAssembly(assembly)
				.That()
				.ResideInNamespaceContaining("Commands")
				.Should()
				.HaveNameEndingWith("Command")
				.And()
				.MeetCustomRule(commandValidationRule)
				.GetResult();
	
		// Assert
		result.IsSuccessful.Should().BeTrue("All commands should implement IRequest or IValidatable interfaces");
	}
}

internal class CommandValidationRule : NetArchTest.Rules.ICustomRule
{
    public bool MeetsRule(Type type)
    {
        // Example: Ensure type implements IRequest or IValidatable
        // Replace with actual interfaces as appropriate for your codebase!
        var implementsIRequest = type.GetInterfaces().Any(i => i.Name == "IRequest");
        var implementsIValidatable = type.GetInterfaces().Any(i => i.Name == "IValidatable");
        return implementsIRequest || implementsIValidatable;
    }

		public bool MeetsRule(TypeDefinition type)
		{
			throw new NotImplementedException();
		}

}