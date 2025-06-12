// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     MyMediatorIntegrationTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  MyMediator.Tests.Unit
// =======================================================

namespace MyMediator;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(MyMediator))]
public class MyMediatorIntegrationTests
{

	[Fact]
	public async Task CompleteFlow_ShouldWorkCorrectly()
	{
		// Arrange
		var services = new ServiceCollection();
		services.AddMyMediator(typeof(MyMediatorIntegrationTests).Assembly);
		var provider = services.BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true });
		var sender = provider.GetRequiredService<ISender>();
		var testRequest = new TestRequest { Value = "integration" };

		// Act
		var result = await sender.Send(testRequest, CancellationToken.None);

		// Assert
		result.Should().Be("INTEGRATION");
	}

	[Fact]
	public async Task MultipleRequests_ShouldWorkIndependently()
	{
		// Arrange
		var services = new ServiceCollection();
		services.AddMyMediator(typeof(MyMediatorIntegrationTests).Assembly);
		var provider = services.BuildServiceProvider();
		var sender = provider.GetRequiredService<ISender>();

		var stringRequest = new TestRequest { Value = "test" };
		var numberRequest = new NumberRequest { Value = 21 };

		// Act
		var stringResult = await sender.Send(stringRequest, CancellationToken.None);
		var numberResult = await sender.Send(numberRequest, CancellationToken.None);

		// Assert
		stringResult.Should().Be("TEST");
		numberResult.Should().Be(42);
	}

	[Fact]
	public async Task CancellationToken_ShouldBePropagated()
	{
		// Arrange
		var services = new ServiceCollection();
		services.AddScoped<IRequestHandler<CancellableRequest, bool>, CancellableRequestHandler>();
		services.AddScoped<ISender, Sender>();
		var provider = services.BuildServiceProvider();
		var sender = provider.GetRequiredService<ISender>();

		var request = new CancellableRequest();
		using var cts = new CancellationTokenSource();
		cts.Cancel();

		// Act
		var act = () => sender.Send(request, cts.Token);

		// Assert
		await act.Should().ThrowAsync<OperationCanceledException>();
	}

}

[ExcludeFromCodeCoverage]
public class CancellableRequest : IRequest<bool> { }

[ExcludeFromCodeCoverage]
public class CancellableRequestHandler : IRequestHandler<CancellableRequest, bool>
{

	public async Task<bool> Handle(CancellableRequest request, CancellationToken cancellationToken)
	{
		await Task.Delay(100, cancellationToken);

		return true;
	}

}