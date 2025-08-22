// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     AuthorDtoValidatorTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain.Tests.Unit
// =======================================================

namespace Domain.Validators;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(AuthorDtoValidator))]
public class AuthorDtoValidatorTests
{

	[Fact]
	public void Should_Fail_When_UserName_Is_Empty()
	{

		// Arrange
		var validator = new AuthorDtoValidator();

		var dto = new AuthorDto
				{ Id = "1", UserName = string.Empty, Email = "test@email.com", Roles = ["Admin"] };

		// Act
		var result = validator.Validate(dto);

		// Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().Contain(e => e.PropertyName == "UserName");

	}

	[Fact]
	public void Should_Fail_When_Email_Is_Empty()
	{

		// Arrange
		var validator = new AuthorDtoValidator();

		var dto = new AuthorDto
				{ Id = "1", UserName = "user", Email = string.Empty, Roles = ["Admin"] };

		// Act
		var result = validator.Validate(dto);

		// Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().Contain(e => e.PropertyName == "Email");

	}

	[Fact]
	public void Should_Fail_When_Email_Is_Invalid()
	{

		// Arrange
		var validator = new AuthorDtoValidator();

		var dto = new AuthorDto
				{ Id = "1", UserName = "user", Email = "not_an_email", Roles = ["Admin"] };

		// Act
		var result = validator.Validate(dto);

		// Assert
		result.IsValid.Should().BeFalse();
		result.Errors.Should().Contain(e => e.PropertyName == "Email");

	}

	[Fact]
	public void Should_Pass_With_Valid_Fields()
	{

		// Arrange
		var validator = new AuthorDtoValidator();

		var dto = new AuthorDto
				{ Id = "12345", UserName = "user", Email = "test@email.com", Roles = ["Admin"] };

		// Act
		var result = validator.Validate(dto);

		// Assert
		result.IsValid.Should().BeTrue();

	}

}