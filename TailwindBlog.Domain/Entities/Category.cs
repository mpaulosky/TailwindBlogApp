// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Category.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

namespace TailwindBlog.Domain.Entities;

/// <summary>
/// Represents a blog category that can be assigned to posts.
/// </summary>
public class Category : Entity
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Category"/> class.
	/// </summary>
	/// <param name="name">The name of the category.</param>
	/// <param name="description">A description of what the category represents.</param>
	/// <param name="skipValidation">If true, skips validation on construction.</param>
	public Category(string name, string description, bool skipValidation = false)
	{
		Name = name;
		Description = description;
		if (!skipValidation)
		{
			ValidateState();
		}
	}

	// Parameterless constructor for EF Core and serialization
	private Category() { }

	/// <summary>
	/// Gets the name of the category.
	/// </summary>
	[Required(ErrorMessage = "Name is required")]
	[MaxLength(80)]
	public string Name { get; private set; } = string.Empty;

	/// <summary>
	/// Gets the description of what this category represents.
	/// </summary>
	[Required(ErrorMessage = "Description is required")]
	[MaxLength(100)]
	public string Description { get; private set; } = string.Empty;

	/// <summary>
	/// Gets an empty category instance.
	/// </summary>
	public static Category Empty =>
		new(string.Empty, string.Empty, skipValidation: true)
		{
			Id = ObjectId.Empty
		};

	/// <summary>
	/// Updates the basic properties of the category.
	/// </summary>
	/// <param name="name">The new name for the category.</param>
	/// <param name="description">The new description for the category.</param>
	/// <prram name="skipValidation">If true, skips validation on update.</prram>
	/// <exception cref="ValidationException">Thrown when the name or description is empty or whitespace.</exception>
	public void Update(string name, string description)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ValidationException("Name is required");
		if (string.IsNullOrWhiteSpace(description))
			throw new ValidationException("Description is required");

		Name = name;
		Description = description;
		ModifiedOn = DateTime.Now;
		ValidateState();
	}

	public void UpdateName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ValidationException("Name is required");

		Name = name;
		ModifiedOn = DateTime.Now;
		ValidateState();
	}

	/// <summary>
	/// Validates the current state of the category.
	/// </summary>
	/// <exception cref="ValidationException">Thrown when validation fails.</exception>
	public void ValidateState()
	{
		var validator = new CategoryValidator();
		var validationResult = validator.Validate(this);
		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult.Errors);
		}
	}
}