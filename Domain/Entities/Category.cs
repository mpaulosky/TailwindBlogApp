// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Categories.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain
// =======================================================

namespace Domain.Entities;

/// <summary>
///   Represents a blog category that can be assigned to posts.
/// </summary>
public class Category : Entity
{

	/// <summary>
	///   Parameterless constructor for serialization and test data generation.
	/// </summary>
	public Category() : this(string.Empty, string.Empty, true) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="Category" /> class.
	/// </summary>
	/// <param name="name">The name of the category.</param>
	/// <param name="slug">A slug of what the category represents.</param>
	/// <param name="skipValidation">If true, skips validation on construction.</param>
	/// <exception cref="ValidationException">Thrown when validation fails</exception>
	public Category(string name, string slug, bool skipValidation = false)
	{
		Name = name;
		Slug = slug;

		if (!skipValidation)
		{
			ValidateState();
		}
	}

	/// <summary>
	///   Gets the name of the category.
	/// </summary>
	[Required(ErrorMessage = "Name is required")]
	[MaxLength(80)]
	public string Name { get; set; }

	/// <summary>
	///   Gets the slug of what this category represents.
	/// </summary>
	[Required(ErrorMessage = "Slug is required")]
	[MaxLength(100)]
	public string Slug { get; set; }

	/// <summary>
	///   Gets an empty category instance.
	/// </summary>
	public static Category Empty =>
			new(string.Empty, string.Empty, true)
			{
				Id = ObjectId.Empty
			};

	/// <summary>
	///   Updates the basic properties of the category.
	/// </summary>
	/// <param name="name">The new name for the category.</param>
	/// <param name="slug">The new slug for the category.</param>
	/// <prram name="skipValidation">If true, skips validation on update.</prram>
	/// <exception cref="ValidationException">Thrown when the name or slug is empty or whitespace.</exception>
	public void Update(string name, string slug)
	{
		
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ValidationException("Name is required");
		}

		if (string.IsNullOrWhiteSpace(slug))
		{
			throw new ValidationException("Slug is required");
		}

		Name = name;
		Slug = slug;
		ModifiedOn = DateTime.Now;
		ValidateState();
		
	}

	/// <summary>
	///   Validates the current state of the category.
	/// </summary>
	/// <exception cref="ValidationException">Thrown when validation fails.</exception>
	private void ValidateState()
	{
		var validator = new CategoryValidator();
		var validationResult = validator.Validate(this);

		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult.Errors);
		}
	}

}