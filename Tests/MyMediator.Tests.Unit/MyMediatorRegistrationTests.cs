// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     MyMediatorRegistrationTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  MyMediator.Tests.Unit
// =======================================================

namespace MyMediator;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(MyMediator))]
public class MyMediatorRegistrationTests
{

	[Fact]
	public void AddMyMediator_ShouldRegisterRequiredServices()
	{
		// Arrange
		var services = new ServiceCollection();

		// Act
		services.AddMyMediator(typeof(MyMediatorRegistrationTests).Assembly);

		// Assert
		var provider = services.BuildServiceProvider();
		var sender = provider.GetService<ISender>();
		sender.Should().NotBeNull();
		sender.Should().BeOfType<Sender>();
	}

	[Fact]
	public void AddMyMediator_WithHandler_ShouldRegisterHandler()
	{
		// Arrange
		var services = new ServiceCollection();

		// Act
		services.AddMyMediator(typeof(TestRequest).Assembly);

		// Assert
		var provider = services.BuildServiceProvider();
		var handlerType = typeof(IRequestHandler<TestRequest, string>);
		var handler = provider.GetService(handlerType);
		handler.Should().NotBeNull();
		handler.Should().BeOfType<TestRequestHandler>();
	}

}

// Test types for handler registration
[ExcludeFromCodeCoverage]
public class TestRequest : IRequest<string>
{

	public string Value { get; init; } = string.Empty;

}

[ExcludeFromCodeCoverage]
public class TestRequestHandler : IRequestHandler<TestRequest, string>
{

	public Task<string> Handle(TestRequest request, CancellationToken cancellationToken)
	{
		return Task.FromResult(request.Value.ToUpper());
	}

}