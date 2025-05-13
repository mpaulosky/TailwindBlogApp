// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Article.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain
// =======================================================

namespace TailwindBlog.Domain.Entities;

[BsonCollection("articles")]
public class Article : Entity
{
	public Article(string title, string introduction, string coverImageUrl, string urlSlug, AppUserModel author, bool isPublished = false, DateTime? publishedOn = null)
	{
		Title = title;
		Introduction = introduction;
		CoverImageUrl = coverImageUrl;
		UrlSlug = urlSlug;
		Author = author;
		IsPublished = isPublished;
		PublishedOn = publishedOn;
	}

	// Parameterless constructor for EF Core and serialization
	private Article() { }

	[Required(ErrorMessage = "Title is required")]
	[MaxLength(100)]
	public string Title { get; private set; } = string.Empty;

	[Required(ErrorMessage = "Introduction is required")]
	[MaxLength(200)]
	public string Introduction { get; private set; } = string.Empty;

	[Required(ErrorMessage = "Cover image is required")]
	[MaxLength(200)]
	[Display(Name = "Cover Image URL")]
	public string CoverImageUrl { get; private set; } = string.Empty;

	[Display(Name = "Url Slug")]
	[MaxLength(200)]
	public string UrlSlug { get; private set; } = string.Empty;

	[Required(ErrorMessage = "Author is required")]
	public AppUserModel Author { get; private set; } = AppUserModel.Empty;

	[Display(Name = "Is Published")]
	public bool IsPublished { get; private set; }

	[Display(Name = "Published On")]
	public DateTime? PublishedOn { get; private set; }

	public static Article Empty =>
		new(string.Empty, string.Empty, string.Empty, string.Empty, AppUserModel.Empty, false, null)
		{
			Id = ObjectId.Empty
		};
}