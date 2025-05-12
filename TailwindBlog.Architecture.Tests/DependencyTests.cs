// =======================================================
// Copyright (c) 2025. All rights reserved.
// Project Name :  TailwindBlog.Architecture.Tests
// =======================================================

using System.Reflection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NetArchTest.Rules;
using Xunit;
using TailwindBlog.ApiService;

namespace TailwindBlog.Architecture.Tests;

public class DependencyTests
{
	[Fact]
	public void MyMediator_Handlers_Should_BeInCorrectNamespace()
	{
		// Arrange
		var assembly = typeof(Program).Assembly;

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
	public void Feature_Classes_Should_BeInCorrectVerticalSlice()
	{
		// Arrange
		var assembly = typeof(Program).Assembly;

		// Act
		var result = Types
				.InAssembly(assembly)
				.That()
				.ResideInNamespaceStartingWith("TailwindBlog.ApiService.Features")
				.Should()
				.BeClasses()
				.And()
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
	public void DependencyInjection_Services_Should_BeRegistered()
	{
		// Arrange
		var services = new ServiceCollection();
		var startup = new Program();

		// Act
		// Get the ConfigureServices method through reflection since it's private
		var method = typeof(Program)
				.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
				.FirstOrDefault(m => m.Name == "ConfigureServices");

		method?.Invoke(startup, new object[] { services });

		// Assert
		services.Should().NotBeEmpty("Services should be registered in DI container");
		services.Any(s => s.ServiceType == typeof(MyMediator.MyMediator))
				.Should().BeTrue("MyMediator should be registered");
	}

	[Fact]
	public void Handlers_Should_Have_SingleResponsibility()
	{
		// Arrange
		var assembly = typeof(Program).Assembly;

		// Act
		var handlers = Types
				.InAssembly(assembly)
				.That()
				.ImplementInterface(typeof(IRequestHandler<,>))
				.GetTypes();

		// Assert
		foreach (var handler in handlers)
		{
			// Check that handlers only implement one IRequestHandler interface
			var handlerInterfaces = handler
					.GetInterfaces()
					.Count(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

			handlerInterfaces.Should().Be(1,
					$"Handler {handler.Name} should implement exactly one IRequestHandler interface");
		}
	}

	[Fact]
	public void MyMediator_Dependencies_Should_BeRegisteredAsScoped()
	{
		// Arrange
		var services = new ServiceCollection();
		var startup = new Program();

		// Act
		// Get the ConfigureServices method through reflection since it's private
		var method = typeof(Program)
				.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
				.FirstOrDefault(m => m.Name == "ConfigureServices");

		method?.Invoke(startup, new object[] { services });

		// Assert
		var mediatorDescriptor = services
				.FirstOrDefault(s => s.ServiceType == typeof(MyMediator.MyMediator));

		mediatorDescriptor.Should().NotBeNull("MyMediator should be registered");
		mediatorDescriptor?.Lifetime.Should().Be(ServiceLifetime.Scoped,
				"MyMediator should be registered as scoped");
	}
}
