// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     FakeArticleDtoTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain.Tests.Unit
// =======================================================

namespace TailwindBlog.Domain.Fakes;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(FakeArticleDto))]
public class FakeArticleDtoTests
{

	[Fact]
	public void GetNewArticleDto_Should_Return_ArticleDto()
	{

		// Arrange & Act
		var dto = FakeArticleDto.GetNewArticleDto();

		// Assert
		dto.Should().NotBeNull();

	}

	[Fact]
	public void GetArticleDtos_Should_Return_Correct_Count()
	{

		// Arrange & Act
		var dtos = FakeArticleDto.GetArticleDtos(2);

		// Assert
		dtos.Should().HaveCount(2);

	}


}