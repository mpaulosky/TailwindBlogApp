// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Category.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

using FluentValidation;
using TailwindBlog.Domain.Extensions;
using TailwindBlog.Domain.Validators;
using ValidationException = FluentValidation.ValidationException;

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
	public Category(string name, string description)
	{
		Name = name;
		Description = description;
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
	/// Gets the URL-friendly version of the category name.
	/// </summary>
	[Display(Name = "Url Slug")]
	[MaxLength(100)]
	public string UrlSlug { get; private set; } = string.Empty;

	/// <summary>
	/// Gets a value indicating whether this category should be shown in navigation menus.
	/// </summary>
	[Display(Name = "Show On Navigation")]
	public bool ShowOnNavigation { get; private set; }

	/// <summary>
	/// Gets an empty category instance.
	/// </summary>
	public static Category Empty =>
		new(string.Empty, string.Empty)
		{
			Id = Guid.Empty
		};

	/// <summary>
	/// Updates the basic properties of the category.
	/// </summary>
	/// <param name="name">The new name for the category.</param>
	/// <param name="description">The new description for the category.</param>
	/// <exception cref="ValidationException">Thrown when name or description is empty or whitespace.</exception>
	public void Update(string name, string description)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ValidationException("Name is required");
		if (string.IsNullOrWhiteSpace(description))
			throw new ValidationException("Description is required");

		Name = name;
		Description = description;
		ModifiedOn = DateTime.Now;
	}

	/// <summary>
	/// Updates the category properties including navigation visibility.
	/// </summary>
	/// <param name="name">The new name for the category.</param>
	/// <param name="description">The new description for the category.</param>
	/// <param name="showOnNavigation">Whether to show this category in navigation menus.</param>
	/// <exception cref="ValidationException">Thrown when name or description is empty or whitespace.</exception>
	public void Update(string name, string description, bool showOnNavigation)
	{
		Update(name, description);
		ShowOnNavigation = showOnNavigation;
	}

	/// <summary>
	/// Updates all properties of the category.
	/// </summary>
	/// <param name="name">The new name for the category.</param>
	/// <param name="description">The new description for the category.</param>
	/// <param name="urlSlug">The URL-friendly version of the name.</param>
	/// <param name="showOnNavigation">Whether to show this category in navigation menus.</param>
	/// <exception cref="ValidationException">Thrown when name or description is empty or whitespace.</exception>
	public void Update(string name, string description, string urlSlug, bool showOnNavigation)
	{
		Update(name, description);
		UrlSlug = urlSlug;
		ShowOnNavigation = showOnNavigation;
	}

	/// <summary>
	/// Updates the category properties and automatically generates a URL slug from the name.
	/// </summary>
	/// <param name="name">The new name for the category.</param>
	/// <param name="description">The new description for the category.</param>
	/// <param name="showOnNavigation">Whether to show this category in navigation menus.</param>
	/// <exception cref="ValidationException">Thrown when validation fails.</exception>
	public void UpdateWithSlug(string name, string description, bool showOnNavigation = false)
	{
		Update(name, description);
		UrlSlug = name.GenerateSlug();
		ShowOnNavigation = showOnNavigation;

		var validator = new CategoryValidator();
		var validationResult = validator.Validate(this);
		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult.Errors);
		}
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
