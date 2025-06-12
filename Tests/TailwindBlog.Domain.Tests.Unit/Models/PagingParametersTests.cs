// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     PagingParametersTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain.Tests.Unit
// =======================================================

namespace TailwindBlog.Domain.Models;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(PagingParameters))]
public class PagingParametersTests
{

	[Fact]
	public void PageSize_Should_Not_Exceed_MaxPageSize()
	{

		// Arrange
		var paging = new PagingParameters { PageSize = 100 };

		// Act & Assert
		paging.PageSize.Should().BeLessOrEqualTo(50);

	}

	[Fact]
	public void TryParse_Should_Return_True_For_Empty_String()
	{

		// Arrange & Act
		var result = PagingParameters.TryParse(string.Empty, null, out var parameters);

		// Assert
		result.Should().BeTrue();
		parameters.Should().NotBeNull();

	}

}