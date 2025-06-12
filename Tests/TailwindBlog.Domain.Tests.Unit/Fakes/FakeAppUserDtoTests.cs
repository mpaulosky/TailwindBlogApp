// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     FakeAppUserDtoTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain.Tests.Unit
// =======================================================

namespace TailwindBlog.Domain.Fakes;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(FakeAppUserDto))]
public class FakeAppUserDtoTests
{

	[Fact]
	public void GetNewAppUserDto_Should_Return_AppUserDto()
	{

		// Arrange & Act
		var dto = FakeAppUserDto.GetNewAppUserDto();

		// Assert
		dto.Should().NotBeNull();

	}

	[Fact]
	public void GetAppUserDtos_Should_Return_Correct_Count()
	{

		// Arrange & Act
		var dtos = FakeAppUserDto.GetAppUserDtos(2);

		// Assert
		dtos.Should().HaveCount(2);

	}


}