// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     CategoryDto.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain
// =======================================================

namespace Domain.Models;

/// <summary>
///   Represents a data transfer object for a category.
/// </summary>
public class CategoryDto
{

	/// <summary>
	///   Parameterless constructor for serialization and test data generation.
	/// </summary>
	public CategoryDto() : this(Guid.Empty, string.Empty, DateTime.MinValue, null, true) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="CategoryDto" /> class.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="name">The name of the category.</param>
	/// <param name="modifiedOn"></param>
	/// <param name="skipValidation">If true, skips validation on construction.</param>
	/// <param name="createdOn"></param>
	/// <exception cref="ValidationException">Thrown when validation fails</exception>
	public CategoryDto(
			Guid id,
			string name,
			DateTime createdOn,
			DateTime? modifiedOn,
			bool skipValidation = false)
	{
		Id = id;
		Name = name;
		CreatedOn = createdOn;
		ModifiedOn = modifiedOn;

		if (!skipValidation)
		{
			ValidateState();
		}
	}

	/// <summary>
	///   Gets or sets the unique identifier for the category.
	/// </summary>
	public Guid Id { get; set; }

	/// <summary>
	///   Gets the name of the category.
	/// </summary>
	[Required(ErrorMessage = "Name is required")]
	[MaxLength(80)]
	public string Name { get; set; }

	/// <summary>
	///   Gets or sets the date and time when the category was created.
	/// </summary>
	public DateTime CreatedOn { get; set; }

	/// <summary>
	///   Gets or sets the date and time when the category was last modified.
	/// </summary>
	public DateTime? ModifiedOn { get; set; }

	/// <summary>
	///   Gets an empty category instance.
	/// </summary>
	public static CategoryDto Empty => new(Guid.Empty, string.Empty, DateTime.MinValue, null, true);

	/// <summary>
	///   Validates the current state of the category.
	/// </summary>
	/// <exception cref="ValidationException">Thrown when validation fails.</exception>
	private void ValidateState()
	{
		var validator = new CategoryDtoValidator();
		var validationResult = validator.Validate(this);

		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult.Errors);
		}
	}

}