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
[Table("Categories")]
[Index(nameof(Name), IsUnique = true)]
public class Category : Entity
{

	/// <summary>
	///   Parameterless constructor for serialization and test data generation.
	/// </summary>
	public Category() : this(string.Empty, true) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="Category" /> class.
	/// </summary>
	/// <param name="name">The name of the category.</param>
	/// <param name="skipValidation">If true, skips validation on construction.</param>
	/// <exception cref="ValidationException">Thrown when validation fails</exception>
	public Category(string name, bool skipValidation = false)
	{
		Name = name;

		if (!skipValidation)
		{
			ValidateState();
		}
	}

	/// <summary>
	///   Gets or sets the name of the category.
	/// </summary>
	[Required(ErrorMessage = "Name is required")]
	[MaxLength(80)]
	public string Name { get; set; }

	public ICollection<Article> Articles { get; set; } = new List<Article>();

	/// <summary>
	///   Gets the name of the category.
	/// </summary>
	public static Category Empty =>
			new(string.Empty, true)
			{
					Id = Guid.Empty
			};

	/// <summary>
	///   Updates the basic properties of the category.
	/// </summary>
	/// <param name="name">The new name for the category.</param>
	/// <prram name="skipValidation">If true, skips validation on update.</prram>
	/// <exception cref="ValidationException">Thrown when the name is empty or whitespace.</exception>
	public void Update(string name)
	{
		Name = name;
		ValidateState();
	}

	/// <summary>
	///   Validates the state of the entity using FluentValidation.
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