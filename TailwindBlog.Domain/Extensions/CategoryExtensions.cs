// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name:     CategoryExtensions.cs
// Company:       mpaulosky
// Author:        Matthew
// Solution Name: TailwindBlog
// Project Name:  TailwindBlog.Domain
// =======================================================

using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Results;
using TailwindBlog.Domain.Entities;
using TailwindBlog.Domain.Validators;

namespace TailwindBlog.Domain.Extensions;

/// <summary>
/// Extension methods for the <see cref="Category"/> entity.
/// </summary>
public static class CategoryExtensions
{
	/// <summary>
	/// Generates a URL-friendly slug from the category name.
	/// </summary>
	/// <param name="input">The string to convert to a slug.</param>
	/// <returns>A URL-friendly slug.</returns>
	public static string GenerateSlug(this string input)
	{
		if (string.IsNullOrWhiteSpace(input)) return string.Empty;

		// Convert to lowercase and trim
		var slug = input.ToLowerInvariant().Trim();

		// Replace spaces and special characters with hyphens
		slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
		slug = Regex.Replace(slug, @"\s+", "-");
		slug = Regex.Replace(slug, @"-+", "-");

		return slug.Trim('-');
	}

	/// <summary>
	/// Validates the category using FluentValidation rules.
	/// </summary>
	/// <param name="category">The category to validate.</param>
	/// <returns>A collection of validation failures, empty if validation succeeds.</returns>
	public static IEnumerable<ValidationFailure> Validate(this Category category)
	{
		var validator = new CategoryValidator();
		var validationResult = validator.Validate(category);
		return validationResult.Errors;
	}
}