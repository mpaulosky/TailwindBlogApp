// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     SystemApiTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService.Tests.Integration
// =======================================================

namespace TailwindBlog.ApiService.Tests.Integration;

[ExcludeFromCodeCoverage]
public class SystemApiTests : ApiTestBase
{
	[Fact]
	public async Task HealthCheck_ShouldReturnHealthyStatus()
	{
		// Act
		var response = await _client.GetAsync("/health");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var content = await response.Content.ReadAsStringAsync();
		content.Should().Contain("Healthy");
	}

	[Fact]
	public async Task AliveCheck_ShouldReturnHealthyStatus()
	{
		// Act
		var response = await _client.GetAsync("/alive");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var content = await response.Content.ReadAsStringAsync();
		content.Should().Contain("Healthy");
	}

	[Fact]
	public async Task OpenApi_ShouldReturnValidSpecification()
	{
		// Act
		var response = await _client.GetAsync("/openapi/v1.json");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		response.Content.Headers.ContentType!.MediaType.Should().Be("application/json");

		var content = await response.Content.ReadAsStringAsync();
		content.Should().Contain("openapi");
		content.Should().Contain("info");
		content.Should().Contain("paths");
	}

	[Fact]
	public async Task Swagger_ShouldReturnValidHtml()
	{
		// Act
		var response = await _client.GetAsync("/api-docs");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		response.Content.Headers.ContentType!.MediaType.Should().Be("text/html");

		var content = await response.Content.ReadAsStringAsync();
		content.Should().Contain("swagger");
	}

	[Fact]
	public async Task HealthCheck_ShouldIncludeMongoDbStatus()
	{
		// Act
		var response = await _client.GetAsync("/health");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var content = await response.Content.ReadAsStringAsync();
		content.Should().Contain("mongodb");
	}
}
