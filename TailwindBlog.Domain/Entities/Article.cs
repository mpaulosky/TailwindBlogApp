// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Article.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain
// =======================================================

using FluentValidation;
using TailwindBlog.Domain.Validators;
using ValidationException = FluentValidation.ValidationException;

namespace TailwindBlog.Domain.Entities;

[BsonCollection("articles")]
public class Article : Entity
{
	public Article(string title, string introduction, string coverImageUrl, string urlSlug, AppUserModel author, bool isPublished = false, DateTime? publishedOn = null, bool skipValidation = false)
	{
		Title = title;
		Introduction = introduction;
		CoverImageUrl = coverImageUrl;
		UrlSlug = urlSlug;
		Author = author;
		IsPublished = isPublished;
		PublishedOn = publishedOn;
		if (!skipValidation)
		{
			ValidateState();
		}
	}

	// Parameterless constructor for EF Core and serialization
	private Article() { }
	public void Update(string title, string introduction, string coverImageUrl, string urlSlug, AppUserModel author, bool isPublished, DateTime? publishedOn)
	{
		Title = title;
		Introduction = introduction;
		CoverImageUrl = coverImageUrl;
		UrlSlug = urlSlug;
		Author = author;
		IsPublished = isPublished;
		PublishedOn = publishedOn;
		ModifiedOn = DateTime.Now;
		ValidateState();
	}

	[Required(ErrorMessage = "Title is required")]
	[MaxLength(100)] public string Title { get; protected internal set; } = string.Empty;

	[Required(ErrorMessage = "Introduction is required")]
	[MaxLength(200)]
	public string Introduction { get; protected internal set; } = string.Empty;

	[Required(ErrorMessage = "Cover image is required")]
	[MaxLength(200)]
	[Display(Name = "Cover Image URL")]
	public string CoverImageUrl { get; protected internal set; } = string.Empty;

	[Display(Name = "Url Slug")]
	[MaxLength(200)]
	public string UrlSlug { get; protected internal set; } = string.Empty;

	[Required(ErrorMessage = "Author is required")]
	public AppUserModel Author { get; protected internal set; } = AppUserModel.Empty;

	[Display(Name = "Is Published")]
	public bool IsPublished { get; protected internal set; }

	[Display(Name = "Published On")]
	public DateTime? PublishedOn { get; protected internal set; }
	public static Article Empty =>
		new(string.Empty, string.Empty, string.Empty, string.Empty, AppUserModel.Empty, false, null, true)
		{
			Id = Guid.Empty
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