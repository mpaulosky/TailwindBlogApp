// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     ArticleModel.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Web
// =======================================================

using MongoDB.Bson;

using TailwindBlog.Domain.Models;

namespace TailwindBlog.Web.Components.Features.Articles.Models;

public class ArticleModel
{

	public ObjectId Id { get; set; }

	public string Title { get; set; } = string.Empty;

	public string Introduction { get; set; } = string.Empty;

	public string CoverImageUrl { get; set; } = string.Empty;

	public string UrlSlug { get; set; } = string.Empty;

	public AppUserModel Author { get; init; } = AppUserModel.Empty;

	public bool IsPublished { get; set; }

	public DateTime? PublishedOn { get; set; }

	public DateTime CreatedOn { get; set; } = DateTime.Now;

	public DateTime? ModifiedOn { get; set; }

	public bool CanEdit { get; set; } = false;

}
