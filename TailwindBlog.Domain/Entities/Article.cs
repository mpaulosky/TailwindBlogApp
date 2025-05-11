// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Article.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain
// =======================================================

namespace TailwindBlog.Domain.Entities;

[Collection("articles")]
public class Article : Entity
{

	[Required(ErrorMessage = "Title is required"),
	MaxLength(100)]
	public string Title { get; set; } = string.Empty;

	[Required(ErrorMessage = "Introduction is required"),
	MaxLength(200)]
	public string Introduction { get; set; } = string.Empty;

	[Required(ErrorMessage = "Cover image is required"),
	MaxLength(200),
	Display(Name = "Cover Image URL")]
	public string CoverImageUrl { get; set; } = string.Empty;

	[Display(Name = "Url Slug"), MaxLength(200)]
	public string UrlSlug { get; set; } = string.Empty;

	[Required(ErrorMessage = "Author is required")]
	public required AppUserModel Author { get; set; } = AppUserModel.Empty;

	[Display(Name = "Is Published")] public bool IsPublished { get; set; }

	[Display(Name = "Published On")] public DateTime? PublishedOn { get; set; }

	public static Article Empty =>
			new()
			{
					Id = ObjectId.Empty,
					Title = string.Empty,
					Introduction = string.Empty,
					CoverImageUrl = string.Empty,
					UrlSlug = string.Empty,
					Author = AppUserModel.Empty,
					IsPublished = false,
					PublishedOn = null
			};

}
