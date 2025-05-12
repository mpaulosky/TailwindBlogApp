// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     SenderTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  MyMediator.Tests.Unit
// =======================================================

namespace MyMediator;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(Sender))]
public class SenderTests
{

	[Fact]
	public async Task Send_WithValidRequest_ShouldReturnResponse()
	{
		// Arrange
		var services = new ServiceCollection();
		var testRequest = new TestRequest { Value = "test" };
		services.AddScoped<IRequestHandler<TestRequest, string>, TestRequestHandler>();
		var provider = services.BuildServiceProvider();
		var sender = new Sender(provider);

		// Act
		var result = await sender.Send(testRequest, CancellationToken.None);

		// Assert
		result.Should().Be("TEST");
	}

	[Fact]
	public async Task Send_WithMultipleHandlers_ShouldResolveCorrectHandler()
	{
		// Arrange
		var services = new ServiceCollection();
		var numberRequest = new NumberRequest { Value = 42 };
		services.AddScoped<IRequestHandler<TestRequest, string>, TestRequestHandler>();
		services.AddScoped<IRequestHandler<NumberRequest, int>, NumberRequestHandler>();
		var provider = services.BuildServiceProvider();
		var sender = new Sender(provider);

		// Act
		var result = await sender.Send(numberRequest, CancellationToken.None);

		// Assert
		result.Should().Be(84);
	}

	[Fact]
	public async Task Send_WithMissingHandler_ShouldThrowException()
	{
		// Arrange
		var services = new ServiceCollection();
		var testRequest = new TestRequest { Value = "test" };
		var provider = services.BuildServiceProvider();
		var sender = new Sender(provider);

		// Act
		var act = () => sender.Send(testRequest, CancellationToken.None);

		// Assert
		await act.Should().ThrowAsync<InvalidOperationException>();
	}

}

[ExcludeFromCodeCoverage]
public class NumberRequest : IRequest<int>
{

	public int Value { get; set; }

}

[ExcludeFromCodeCoverage]
public class NumberRequestHandler : IRequestHandler<NumberRequest, int>
{

	public Task<int> Handle(NumberRequest request, CancellationToken cancellationToken)
	{
		return Task.FromResult(request.Value * 2);
	}

}
