// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArticleModel.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : TailwindBlog
// Project Name :  Web
// =======================================================

using System.ComponentModel.DataAnnotations;

namespace Web.Components.Features.Articles.Models;

public class ArticleModel
{

	/// <summary>
	///   Gets or sets the unique identifier for the article.
	/// </summary>
	public ObjectId Id { get; set; } = ObjectId.Empty;

	/// <summary>
	///   Gets or sets the title of the article.
	/// </summary>
	public string Title { get; set; } = string.Empty;

	/// <summary>
	///   Gets or sets the introduction or summary of the article.
	/// </summary>
	public string Introduction { get; set; } = string.Empty;

	/// <summary>
	///   Gets or sets the main content of the article.
	/// </summary>
	public string Content { get; set; } = string.Empty;

	/// <summary>
	///   Gets or sets the URL of the article's cover image.
	/// </summary>
	public string CoverImageUrl { get; set; } = string.Empty;

	/// <summary>
	///   Gets or sets the URL-friendly slug for the article.
	/// </summary>
	public string UrlSlug { get; set; } = string.Empty;

	/// <summary>
	///   Gets or sets the author information of the article.
	/// </summary>
	public AppUserDto Author { get; set; } = AppUserDto.Empty;

	/// <summary>
	///   Gets or sets the category information of the article.
	/// </summary>
	public CategoryDto Category { get; set; } = CategoryDto.Empty;

	/// <summary>
	///   Gets the date and time when this entity was created.
	/// </summary>)]
	[Display(Name = "Created On")]
	public DateTime CreatedOn { get; set; } = DateTime.MinValue;

	/// <summary>
	///   Gets or sets the date and time when this entity was last modified.
	/// </summary>
	[Display(Name = "Modified On")]
	public DateTime? ModifiedOn { get; set; } = null;

	/// <summary>
	///   Gets or sets a value indicating whether the article is published.
	/// </summary>
	public bool IsPublished { get; set; } = false;

	/// <summary>
	///   Gets or sets the date when the article was published.
	/// </summary>
	[Display(Name = "Published On")]
	public DateTime? PublishedOn { get; set; } = null;

	/// <summary>
	///   Gets or sets a value indicating whether the article is marked as deleted.
	/// </summary>
	public bool IsDeleted { get; set; } = false;

	public bool CanEdit { get; set; } = false;

}