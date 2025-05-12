// =======================================================
// Copyright (c) 2025. All rights reserved.
// Project Name :  TailwindBlog.Architecture.Tests
// =======================================================

using System.Reflection;
using FluentAssertions;
using MyMediator;
using NetArchTest.Rules;
using TailwindBlog.Domain.Abstractions;
using static TailwindBlog.Architecture.Tests.AssemblyReference;
using Xunit;

namespace TailwindBlog.Architecture.Tests;

public class CqrsImplementationTests
{
	[Fact(DisplayName = "CQRS Test: Commands should modify state")]
	public void Commands_Should_Modify_State()
	{
		// Arrange
		var assembly = ApiService;

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
	public void Queries_Should_Be_ReadOnly()
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
				.NotHaveDependencyOnAny(
						"TailwindBlog.ApiService.Features.Commands",
						"TailwindBlog.Persistence.Repositories")
				.And()
				.BeImmutable()
				.GetResult();

		// Assert
		result.IsSuccessful.Should().BeTrue(
				"Queries should not modify state and should be immutable");
	}

	[Fact(DisplayName = "CQRS Test: Commands and Queries should use MyMediatr")]
	public void Commands_And_Queries_Should_Use_MyMediatr()
	{
		// Arrange
		var assembly = ApiService;

		// Act
		var result = Types
				.InAssembly(assembly)
				.That()
				.ResideInNamespaceContaining("Commands")
				.Or()
				.ResideInNamespaceContaining("Queries")
				.Should()
				.HaveDependencyOn("MyMediator")
				.GetResult();

		// Assert  
		result.IsSuccessful.Should().BeTrue(
				"Commands and Queries should use MyMediatr for handling");
	}

	[Fact(DisplayName = "CQRS Test: Features should have proper Request/Handler pattern")]
	public void Features_Should_Have_Request_Handler_Pattern()
	{
		// Arrange
		var assembly = ApiService;

		// Act
		var requestHandlerTypes = Types
				.InAssembly(assembly)
				.That()
				.ImplementInterface(typeof(IRequestHandler<,>))
				.GetTypes();

		// Assert
		foreach (var handlerType in requestHandlerTypes)
		{
			// Find corresponding request type
			var handlerInterface = handlerType.GetInterfaces()
					.First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

			var requestType = handlerInterface.GetGenericArguments()[0];

			// Verify naming convention
			handlerType.Name.Should().Be($"{requestType.Name}Handler",
					"Handler class name should be RequestNameHandler");

			// Verify request properties
			var properties = requestType.GetProperties();
			properties.Should().NotBeEmpty(
					$"Request type {requestType.Name} should have at least one property");
		}
	}
}
