// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Article.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain
// =======================================================

namespace Domain.Entities;

/// <summary>
///   Represents an article in the blog system.
/// </summary>
public class Article : Entity
{

	/// <summary>
	///   Parameterless constructor for serialization and test data generation.
	/// </summary>
	public Article() : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, AppUserDto.Empty, CategoryDto.Empty, false, null, true) { }
	/// <summary>
	///   Initializes a new instance of the <see cref="Article" /> class.
	/// </summary>
	/// <param name="title">The new title</param>
	/// <param name="introduction">The new introduction</param>
	/// <param name="content">The new content</param>
	/// <param name="coverImageUrl">The new cover image URL</param>
	/// <param name="urlSlug">The new URL slug</param>
	/// <param name="author">The new author</param>
	/// <param name="category">The new category</param>
	/// <param name="isPublished">The newly published status</param>
	/// <param name="publishedOn">The new publication date</param>
	/// <param name="skipValidation">If true, skips validation on construction.</param>
	/// <exception cref="ValidationException">Thrown when validation fails</exception>
	public Article(
			string title,
			string introduction,
			string content,
			string coverImageUrl,
			string urlSlug,
			AppUserDto author,
			CategoryDto category,
			bool isPublished = false,
			DateTime? publishedOn = null,
			bool skipValidation = false)
	{
		Title = title;
		Introduction = introduction;
		Content = content;
		CoverImageUrl = coverImageUrl;
		UrlSlug = urlSlug;
		Author = author;
		Category = category;
		IsPublished = isPublished;
		PublishedOn = publishedOn;

		if (!skipValidation)
		{
			ValidateState();
		}
	}

	/// <summary>
	///   Updates the article's properties
	/// </summary>
	/// <param name="title">The new title</param>
	/// <param name="introduction">The new introduction</param>
	/// <param name="content"></param>
	/// <param name="coverImageUrl">The new cover image URL</param>
	/// <param name="urlSlug">The new URL slug</param>
	/// <param name="author">The new author</param>
	/// <param name="category">The new category</param>
	/// <param name="isPublished">The newly published status</param>
	/// <param name="publishedOn">The new publication date</param>
	/// <exception cref="ValidationException">Thrown when validation fails</exception>
	public void Update(
			string title,
			string introduction,
			string content,
			string coverImageUrl,
			string urlSlug,
			AppUserDto author,
			CategoryDto category,
			bool isPublished,
			DateTime? publishedOn)
	{
		Title = title;
		Introduction = introduction;
		Content = content;
		CoverImageUrl = coverImageUrl;
		UrlSlug = urlSlug;
		Author = author;
		Category = category;
		IsPublished = isPublished;
		PublishedOn = publishedOn;
		ModifiedOn = DateTime.UtcNow;
		ValidateState();
	}

	public void Publish(DateTime publishedOn)
	{
		IsPublished = true;
		PublishedOn = publishedOn;
		ModifiedOn = DateTime.UtcNow;
		ValidateState();
	}

	public void Unpublish()
	{
		IsPublished = false;
		PublishedOn = null;
		ModifiedOn = DateTime.UtcNow;
		ValidateState();
	}

	// The properties without MaxLength attributes
	/// <summary>
	///   Gets or sets the title of the article.
	/// </summary>
	public string Title { get; set; }

	/// <summary>
	///   Gets or sets the introduction/summary of the article.
	/// </summary>
	public string Introduction { get; set; }

	/// <summary>
	///   Gets or sets the main content of the article.
	///   See <see cref="ArticleDtoValidator" /> for validation rules.
	/// </summary>
	public string Content { get; set; }

	/// <summary>
	///   Gets or sets the URL of the cover image.
	/// </summary>
	[Display(Name = "Cover Image URL")]
	public string CoverImageUrl { get; set; }

	/// <summary>
	///   Gets or sets the URL slug for the article.
	/// </summary>
	[Display(Name = "Url Slug")]
	public string UrlSlug { get; set; }

	/// <summary>
	///   Gets or sets the author of the article.
	/// </summary>
	public AppUserDto Author { get; set; }

	/// <summary>
	///   Gets or sets the category of the article.
	/// </summary>
	public CategoryDto Category { get; set; }

	/// <summary>
	///   Gets or sets whether the article is published.
	/// </summary>
	[Display(Name = "Is Published")]
	public bool IsPublished { get; set; }

	/// <summary>
	///   Gets or sets the date when the article was published.
	/// </summary>
	[Display(Name = "Published On")]
	public DateTime? PublishedOn { get; set; }

	/// <summary>
	///   Gets an empty article instance.
	/// </summary>
	public static Article Empty { get; } = new(
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			AppUserDto.Empty,
			CategoryDto.Empty)
	{
		Id = ObjectId.Empty
	};

	private void ValidateState()
	{
		var validator = new ArticleValidator();
		var validationResult = validator.Validate(this);

		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult.Errors);
		}
	}

}