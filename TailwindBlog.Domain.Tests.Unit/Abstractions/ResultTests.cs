// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ResultTests.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain.Tests.Unit
// =======================================================

namespace TailwindBlog.Domain.Abstractions;

[ExcludeFromCodeCoverage]
[TestSubject(typeof(Result))]
public class ResultTests
{
	[Fact]
	public void Result_WhenCreatedWithSuccess_ShouldHaveCorrectProperties()
	{
		// Arrange & Act
		var result = new Result(true);

		// Assert
		result.Success.Should().BeTrue();
		result.Failure.Should().BeFalse();
		result.Error.Should().BeNull();
	}

	[Fact]
	public void Result_WhenCreatedWithFailure_ShouldHaveCorrectProperties()
	{
		// Arrange
		const string errorMessage = "Test error";

		// Act
		var result = new Result(false, errorMessage);

		// Assert
		result.Success.Should().BeFalse();
		result.Failure.Should().BeTrue();
		result.Error.Should().Be(errorMessage);
	}

	[Fact]
	public void Ok_ShouldCreateSuccessResult()
	{
		// Arrange & Act
		var result = Result.Ok();

		// Assert
		result.Success.Should().BeTrue();
		result.Error.Should().BeNull();
	}

	[Fact]
	public void Fail_ShouldCreateFailureResult()
	{
		// Arrange
		const string errorMessage = "Test error";

		// Act
		var result = Result.Fail(errorMessage);

		// Assert
		result.Success.Should().BeFalse();
		result.Error.Should().Be(errorMessage);
	}

	[Fact]
	public void OkT_ShouldCreateSuccessResultWithValue()
	{
		// Arrange
		const string value = "Test value";

		// Act
		var result = Result.Ok(value);

		// Assert
		result.Success.Should().BeTrue();
		result.Error.Should().BeEmpty();
		result.Value.Should().Be(value);
	}

	[Fact]
	public void FailT_ShouldCreateFailureResultWithoutValue()
	{
		// Arrange
		const string errorMessage = "Test error";

		// Act
		var result = Result.Fail<string>(errorMessage);

		// Assert
		result.Success.Should().BeFalse();
		result.Error.Should().Be(errorMessage);
		result.Value.Should().BeNull();
	}

	[Fact]
	public void FromValue_WhenValueIsNotNull_ShouldCreateSuccessResult()
	{
		// Arrange
		const string value = "Test value";

		// Act
		var result = Result.FromValue(value);

		// Assert
		result.Success.Should().BeTrue();
		result.Value.Should().Be(value);
	}

	[Fact]
	public void FromValue_WhenValueIsNull_ShouldCreateFailureResult()
	{
		// Arrange
		string? value = null;

		// Act
		var result = Result.FromValue(value);

		// Assert
		result.Success.Should().BeFalse();
		result.Error.Should().Be("Provided value is null.");
		result.Value.Should().BeNull();
	}

	[Fact]
	public void ImplicitOperator_FromValue_ShouldCreateSuccessResult()
	{
		// Arrange
		const string value = "Test value";

		// Act
		Result<string> result = value;

		// Assert
		result.Success.Should().BeTrue();
		result.Value.Should().Be(value);
	}

	[Fact]
	public void ImplicitOperator_ToValue_ShouldReturnValue()
	{
		// Arrange
		const string originalValue = "Test value";
		var result = Result.Ok(originalValue);

		// Act
		string? extractedValue = result;

		// Assert
		extractedValue.Should().Be(originalValue);
	}
}