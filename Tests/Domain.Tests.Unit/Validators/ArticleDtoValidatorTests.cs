// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArticleDtoValidatorTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain.Tests.Unit
// =======================================================

namespace Domain.Validators;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(ArticleDtoValidator))]
public class ArticleDtoValidatorTests
{

	[Fact]
	public void Should_Fail_When_Title_Is_Empty()
	{

		// Arrange
		var validator = new ArticleDtoValidator();

		var dto = FakeArticleDto.GetNewArticleDto();
		dto.Title = string.Empty;

		// Act
		var result = validator.Validate(dto);

		// Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().Contain(e => e.PropertyName == "Title");

	}

	[Fact]
	public void Should_Fail_When_UrlSlug_Is_Invalid()
	{

		// Arrange
		var validator = new ArticleDtoValidator();
		var dto = FakeArticleDto.GetNewArticleDto();
		dto.UrlSlug = "Invalid Slug!";

		// Act
		var result = validator.Validate(dto);

		// Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().Contain(e => e.PropertyName == "UrlSlug");

	}

	[Fact]
	public void Should_Pass_With_Valid_Fields()
	{

		// Arrange
		var validator = new ArticleDtoValidator();
		var dto = FakeArticleDto.GetNewArticleDto();

		// Act
		var result = validator.Validate(dto);

		// Assert
		result.IsValid.Should().BeTrue();

	}

}