// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     FakeCategoryDtoTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain.Tests.Unit
// =======================================================

namespace TailwindBlog.Domain.Fakes;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(FakeCategoryDto))]
public class FakeCategoryDtoTests
{

	[Fact]
	public void GetNewCategoryDto_Should_Return_CategoryDto()
	{

		// Arrange & Act
		var dto = FakeCategoryDto.GetNewCategoryDto();

		// Assert
		dto.Should().NotBeNull();

	}

	[Fact]
	public void GetCategoriesDto_Should_Return_Correct_Count()
	{

		// Arrange & Act
		var dtos = FakeCategoryDto.GetCategoriesDto(2);

		// Assert
		dtos.Should().HaveCount(2);

	}


}