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
[Table("Articles")]
[Index(nameof(UrlSlug), IsUnique = true)]
public class Article : Entity
{

	/// <summary>
	///   Initializes a new instance of the <see cref="Article" /> class.
	/// </summary>
	/// <param name="category">The category associated with the article.</param>
	/// <param name="author"></param>
	public Article(Category category, Author author) : this(string.Empty, string.Empty, string.Empty, string.Empty,
			string.Empty, string.Empty, Guid.Empty, category, author, false, null, true) { }

	/// <summary>
	///   Initializes a new instance of the <see cref="Article" /> class with specified values.
	/// </summary>
	/// <param name="title">The title of the article.</param>
	/// <param name="introduction">The introduction of the article.</param>
	/// <param name="content">The main content of the article.</param>
	/// <param name="coverImageUrl">The URL of the cover image for the article.</param>
	/// <param name="urlSlug">The URL slug for the article.</param>
	/// <param name="authorId">The author ID of the article.</param>
	/// <param name="categoryId">The category ID of the article.</param>
	/// <param name="category">The category associated with the article.</param>
	/// <param name="author">The author associated with the article.</param>
	/// <param name="isPublished">A value indicating whether the article is published.</param>
	/// <param name="publishedOn">The date and time when the article was published.</param>
	/// <param name="skipValidation">A value indicating whether to skip validation.</param>
	public Article(
			string title,
			string introduction,
			string content,
			string coverImageUrl,
			string urlSlug,
			string authorId,
			Guid categoryId,
			Category category,
			Author author,
			bool isPublished = false,
			DateTime? publishedOn = null,
			bool skipValidation = false)
	{
		Title = title;
		Introduction = introduction;
		Content = content;
		CoverImageUrl = coverImageUrl;
		UrlSlug = urlSlug;
		AuthorId = authorId;
		CategoryId = categoryId;
		Category = category;
		Author = author;
		IsPublished = isPublished;
		PublishedOn = publishedOn;

		if (!skipValidation)
		{
			ValidateState();
		}
	}

	/// <summary>
	///   Gets or sets the title of the article.
	/// </summary>
	[Required]
	[MaxLength(120)]
	public string Title { get; set; }

	/// <summary>
	///   Gets or sets the introduction of the article.
	/// </summary>
	[MaxLength(250)]
	public string Introduction { get; set; }

	/// <summary>
	///   Gets or sets the main content of the article.
	/// </summary>
	[Required]
	[MaxLength(5000)]
	public string Content { get; set; }

	/// <summary>
	///   Gets or sets the URL of the cover image for the article.
	/// </summary>
	[MaxLength(250)]
	public string CoverImageUrl { get; set; }

	/// <summary>
	///   Gets or sets the URL slug for the article, used in the article's URL.
	/// </summary>
	[Required]
	[MaxLength(120)]
	public string UrlSlug { get; set; }

	/// <summary>
	///   Gets or sets the author ID of the article.
	/// </summary>
	[Required]
	[ForeignKey(nameof(Author))]
	[MaxLength(36)] // Assuming AuthorId is a GUID stored as a string
	public string AuthorId { get; set; }

	/// <summary>
	///   Represents the author entity associated with the article.
	///   This property defines the relationship between an article and its author.
	///   It establishes a navigational property that links the article to the author who created it.
	/// </summary>
	public Author Author { get; set; }

	/// <summary>
	///   Gets or sets the category ID of the article.
	/// </summary>
	[ForeignKey(nameof(Category))]
	public Guid CategoryId { get; set; }

	/// <summary>
	///   Represents the category entity associated with the article.
	///   This property defines the relationship between an article and its category.
	///   It establishes a navigational property that links the article to the category under which it is published.
	/// </summary>
	public Category Category { get; set; }

	/// <summary>
	///   Gets or sets a value indicating whether the article is published.
	/// </summary>
	public bool IsPublished { get; set; }

	public static Article Empty { get; } = new(
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			string.Empty,
			Author.Empty.Id,
			Category.Empty.Id,
			Category.Empty,
			Author.Empty,
			false,
			null,
			true // skipValidation: allow empty instance for tests
	);

	/// <summary>
	///   Gets or sets the date and time when the article was published.
	/// </summary>
	public DateTime? PublishedOn { get; set; }

	public void Update(
			string title,
			string introduction,
			string content,
			string coverImageUrl,
			string urlSlug,
			Category category,
			Guid categoryId,
			bool isPublished = false,
			DateTime? publishedOn = null)
	{
		Title = title;
		Introduction = introduction;
		Content = content;
		CoverImageUrl = coverImageUrl;
		UrlSlug = urlSlug;
		Category = category;
		CategoryId = categoryId;
		IsPublished = isPublished;
		PublishedOn = isPublished ? publishedOn : null;

		// Set ModifiedOn (assumed to be declared in base class or somewhere accessible)
		// If not, please add a public DateTime? ModifiedOn { get; set; } property to Entity
		// For new articles, do not reset CreatedOn
		var modifiedOnProp = GetType().GetProperty("ModifiedOn");

		if (modifiedOnProp != null && modifiedOnProp.CanWrite)
		{
			modifiedOnProp.SetValue(this, DateTime.UtcNow);
		}

		ValidateState();
	}

	public void Publish(DateTime publishDate)
	{
		IsPublished = true;
		PublishedOn = publishDate;
		ModifiedOn = DateTime.UtcNow;
	}

	public void Unpublish()
	{
		IsPublished = false;
		PublishedOn = null;
		ModifiedOn = DateTime.UtcNow;
	}

	/// <summary>
	///   Validates the state of the entity using FluentValidation.
	/// </summary>
	/// <exception cref="ValidationException">Thrown when validation fails.</exception>
	private void ValidateState()
	{

		var validator = new ArticleValidator();

		var validationResult = validator.Validate(this);

		if (!validationResult.IsValid)
		{
			throw new ValidationException(validationResult.Errors);
		}

	}

	/// <summary>
	///   Protected parameterless constructor for EF Core, Bogus, and serialization.
	/// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
	public Article()
	{
		// For Bogus, EF, and serialization
	}

}