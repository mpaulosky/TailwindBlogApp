// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CqrsImplementationTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Architecture.Tests
// =======================================================

namespace TailwindBlog.Architecture.Tests;

public class CqrsImplementationTests
{
	[Fact(DisplayName = "CQRS Test: Commands should modify state")]
	public void Commands_Should_Modify_State()
	{
		// Arrange
		var assembly = AssemblyReference.ApiService;

		// Act
		var result = Types
				.InAssembly(assembly)
				.That()
				.ResideInNamespaceContaining("Commands")
				.And()
				.HaveNameEndingWith("Command")
				.Should()
				.HaveDependencyOn("TailwindBlog.Persistence")
				.GetResult();

		// Assert
		result.IsSuccessful.Should().BeTrue(
				"Commands should interact with persistence to modify state");
	}

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

		var handlerMethods = commandHandlers
				.SelectMany(t => t.GetMethods())
				.Where(m => m.Name == "Handle")
				.ToList();

		// Assert
		handlerMethods.Should().AllSatisfy(method =>
		{
			var returnType = method.ReturnType;
			returnType.IsGenericType.Should().BeTrue();
			returnType.GetGenericTypeDefinition().Should().Be(typeof(Task<>));
			var resultType = returnType.GetGenericArguments()[0];
			resultType.Should().Be(typeof(Result));
		}, "Command handlers should return Result");
	}

	[Fact(DisplayName = "CQRS Test: Queries should be read-only")]
	public void Queries_Should_Be_ReadOnly()
	{
		// Arrange
		var assembly = AssemblyReference.ApiService;

		// Act
		var result = Types
				.InAssembly(assembly)
				.That()
				.ResideInNamespaceContaining("Queries")
				.Should()
				.NotHaveDependencyOn("Commands")
				.And()
				.NotHaveDependencyOn("Persistence")
				.GetResult();

		// Assert
		result.IsSuccessful.Should().BeTrue(
				"Queries should not modify state");
	}

	[Fact(DisplayName = "CQRS Test: Queries should return domain objects")]
	public void Queries_Should_Return_Domain_Objects()
	{
		// Arrange
		var assembly = AssemblyReference.ApiService;

		// Act
		var queryHandlers = Types
				.InAssembly(assembly)
				.That()
				.ResideInNamespaceContaining("Queries")
				.And()
				.ImplementInterface(typeof(IRequestHandler<,>))
				.GetTypes();

		var handlerMethods = queryHandlers
				.SelectMany(t => t.GetMethods())
				.Where(m => m.Name == "Handle")
				.ToList();

		// Assert
		handlerMethods.Should().AllSatisfy(method =>
		{
			var returnType = method.ReturnType;
			returnType.IsGenericType.Should().BeTrue();
			returnType.GetGenericTypeDefinition().Should().Be(typeof(Task<>));
			var resultType = returnType.GetGenericArguments()[0];
			resultType.Should().Match(t =>
							t.GetInterfaces().Contains(typeof(IEnumerable)) ||
							t.IsAssignableTo(typeof(Result)));
		}, "Query handlers should return collections or Result types");
	}
}