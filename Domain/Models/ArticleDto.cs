// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArticleDto.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain
// =======================================================

namespace Domain.Models;

/// <summary>
///   Data Transfer Object (DTO) representing an article.
///   All validations are handled by <see cref="ArticleDtoValidator" />.
/// </summary>
public sealed class ArticleDto
{

	/// <summary>
	///   Parameterless constructor for serialization and test data generation.
	/// </summary>
	public ArticleDto() : this(ObjectId.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, AppUserDto.Empty, CategoryDto.Empty, DateTime.MinValue, null, false, null, true) { }
	/// <summary>
	///   Initializes a new instance of the <see cref="ArticleDto" /> class.
	/// </summary>
	/// <param name="id"></param>
	/// <param name="title">The new title</param>
	/// <param name="introduction">The new introduction</param>
	/// <param name="content">The new content</param>
	/// <param name="coverImageUrl">The new cover image URL</param>
	/// <param name="urlSlug">The new URL slug</param>
	/// <param name="author">The new author</param>
	/// <param name="category">The new category</param>
	/// <param name="createdOn">The date the item was created</param>
	/// <param name="modifiedOn">The date the item was modified</param>
	/// <param name="isPublished">The newly published status</param>
	/// <param name="publishedOn">The new publication date</param>
	/// <param name="skipValidation">If true, skips validation on construction.</param>
	/// <exception cref="ValidationException">Thrown when validation fails</exception>
	public ArticleDto(
			ObjectId id,
			string title,
			string introduction,
			string content,
			string coverImageUrl,
			string urlSlug,
			AppUserDto author,
			CategoryDto category,
			DateTime createdOn,
			DateTime? modifiedOn,
			bool isPublished,
			DateTime? publishedOn = null,
			bool skipValidation = false)
	{
		Id = id;
		Title = title;
		Introduction = introduction;
		Content = content;
		CoverImageUrl = coverImageUrl;
		UrlSlug = urlSlug;
		Author = author;
		Category = category;
		CreatedOn = createdOn;
		ModifiedOn = modifiedOn;
		IsPublished = isPublished;
		PublishedOn = publishedOn;

		if (!skipValidation)
		{
			ValidateState();
		}
	}

	/// <summary>
	///   Gets or sets the unique identifier for the article.
	/// </summary>
	public ObjectId Id { get; set; }

	/// <summary>
	///   Gets or sets the title of the article.
	///   See <see cref="ArticleDtoValidator" /> for validation rules.
	/// </summary>
	public string Title { get; set; }

	/// <summary>
	///   Gets or sets the introduction or summary of the article.
	///   See <see cref="ArticleDtoValidator" /> for validation rules.
	/// </summary>
	public string Introduction { get; set; }

	/// <summary>
	///   Gets or sets the main content of the article.
	///   See <see cref="ArticleDtoValidator" /> for validation rules.
	/// </summary>
	public string Content { get; set; }

	/// <summary>
	///   Gets or sets the URL of the article's cover image.
	///   See <see cref="ArticleDtoValidator" /> for validation rules.
	/// </summary>
	public string CoverImageUrl { get; set; }

	/// <summary>
	///   Gets or sets the URL-friendly slug for the article.
	///   See <see cref="ArticleDtoValidator" /> for validation rules.
	/// </summary>
	public string UrlSlug { get; set; }

	/// <summary>
	///   Gets or sets the author information of the article.
	///   See <see cref="ArticleDtoValidator" /> for validation rules.
	/// </summary>
	public AppUserDto Author { get; set; }

	/// <summary>
	///   Gets or sets the category information of the article.
	///   See <see cref="ArticleDtoValidator" /> for validation rules.
	/// </summary>
	public CategoryDto Category { get; set; }

	/// <summary>
	///   Gets the date and time when this entity was created.
	/// </summary>)]
	[Display(Name = "Created On")]
	public DateTime CreatedOn { get; set; }

	/// <summary>
	///   Gets or sets the date and time when this entity was last modified.
	/// </summary>
	[Display(Name = "Modified On")]
	public DateTime? ModifiedOn { get; set; }

	/// <summary>
	///   Gets or sets a value indicating whether the article is published.
	/// </summary>
	public bool IsPublished { get; set; }


	/// <summary>
	///   Gets or sets the date when the article was published.
	/// </summary>
	[Display(Name = "Published On")]
	public DateTime? PublishedOn { get; set; }

	/// <summary>
	///   Gets or sets a value indicating whether the article is marked as deleted.
	/// </summary>
	public bool Archived { get; set; }

	/// <summary>
	///   Gets an empty article instance.
	/// </summary>
	public static ArticleDto Empty { get; } =
		new(
				ObjectId.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				string.Empty,
				AppUserDto.Empty,
				CategoryDto.Empty,
				DateTime.MinValue,
				null,
				false,
				null,
				true
		);

	private void ValidateState()
	{
		var validator = new ArticleDtoValidator();
		var validationResult = validator.Validate(this);

		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult.Errors);
		}
	}

}