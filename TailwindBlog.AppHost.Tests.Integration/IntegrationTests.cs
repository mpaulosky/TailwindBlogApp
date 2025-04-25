namespace TailwindBlog.AppHost.Tests.Integration;

public class IntegrationTests
{

	private static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(50);


	[Fact]
	public async Task GetWebResourceRootReturnsOkStatusCode()
	{

		// Arrange
		var appHost = await DistributedApplicationTestingBuilder.CreateAsync<Projects.TailwindBlog_AppHost>(TestContext.Current.CancellationToken);

		appHost.Services.AddLogging(logging =>
		{
			logging.SetMinimumLevel(LogLevel.Debug);

			// Override the logging filters from the app's configuration
			logging.AddFilter(appHost.Environment.ApplicationName, LogLevel.Debug);
			logging.AddFilter("Aspire.", LogLevel.Debug);

			// To output logs to the xUnit.net ITestOutputHelper, consider adding a package from https://www.nuget.org/packages?q=xunit+logging
		});

		appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
		{
			clientBuilder.AddStandardResilienceHandler();
		});

		await using var app = await appHost.BuildAsync(TestContext.Current.CancellationToken).WaitAsync(_defaultTimeout, TestContext.Current.CancellationToken);

		await app.StartAsync(TestContext.Current.CancellationToken).WaitAsync(_defaultTimeout, TestContext.Current.CancellationToken);

		// Act
		var httpClient = app.CreateHttpClient(WebApp);

		await app.ResourceNotifications.WaitForResourceHealthyAsync(WebApp, TestContext.Current.CancellationToken).WaitAsync(_defaultTimeout, TestContext.Current.CancellationToken);

		var response = await httpClient.GetAsync("/", TestContext.Current.CancellationToken);

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);

	}

}
