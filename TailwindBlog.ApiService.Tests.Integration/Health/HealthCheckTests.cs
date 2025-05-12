// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     HealthCheckTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService.Tests.Integration
// =======================================================

namespace TailwindBlog.ApiService.Tests.Integration.Health;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(Program))]
public class HealthCheckTests : ApiTestBase
{
	[Fact]
	public async Task Health_Check_Returns_Success()
	{
		// Act
		var response = await _client.GetAsync("/health");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var content = await response.Content.ReadAsStringAsync();
		content.Should().Be("Healthy");
	}

	[Fact]
	public async Task Liveness_Check_Returns_Success()
	{
		// Act
		var response = await _client.GetAsync("/alive");

		// Assert
		response.StatusCode.Should().Be(HttpStatusCode.OK);
		var content = await response.Content.ReadAsStringAsync();
		content.Should().Be("Healthy");
	}
}
