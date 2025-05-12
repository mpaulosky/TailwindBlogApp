// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ApiTestBase.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService.Tests.Integration
// =======================================================

using Microsoft.AspNetCore.Hosting;

namespace TailwindBlog.ApiService.Tests.Integration;

[ExcludeFromCodeCoverage]
public class ApiTestBase : IAsyncLifetime
{
	protected readonly WebApplicationFactory<Program> _factory;
	protected readonly MongoDbContainer _mongodb;
	protected readonly HttpClient _client;

	protected ApiTestBase()
	{
		_mongodb = new MongoDbBuilder()
				.WithName($"mongodb-test-{Guid.NewGuid()}")
				.Build();

		_factory = new WebApplicationFactory<Program>()
				.WithWebHostBuilder(builder =>
				{
					builder.ConfigureAppConfiguration((context, config) =>
							{
							config.AddInMemoryCollection(new Dictionary<string, string>
								{
												{"ConnectionStrings:tailwind-blog", _mongodb.GetConnectionString()}
								});
						});

					builder.UseEnvironment("Development");
				});

		_client = _factory.CreateClient();
	}

	public async Task InitializeAsync()
	{
		await _mongodb.StartAsync();
	}

	public async Task DisposeAsync()
	{
		await _mongodb.DisposeAsync();
		await _factory.DisposeAsync();
	}
}
