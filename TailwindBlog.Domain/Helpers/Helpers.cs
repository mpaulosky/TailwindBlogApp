// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Helpers.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain
// =======================================================

using System.Web;

using Bogus;

using MongoDB.Bson;

using TailwindBlog.Domain.Entities;
using TailwindBlog.Domain.Models;

namespace TailwindBlog.Domain.Helpers;

public static class Helpers
{

	public static readonly Faker<Article> ArticleGenerator =
			new Faker<Article>()
					.UseSeed(421)
					.RuleFor(x => x.Id, ObjectId.GenerateNewId)
					.RuleFor(x => x.Title, f => f.WaffleTitle())
					.RuleFor(x => x.UrlSlug, (_, x) => x.Title.GetSlug())
					.RuleFor(x => x.Introduction, f => f.WaffleTitle())
					.RuleFor(x => x.CreatedOn, GetStaticDate())
					.RuleFor(x => x.IsPublished, f => f.Random.Bool())
					.RuleFor(x => x.CreatedOn, GetStaticDate())
					.RuleFor(x => x.PublishedOn, (_, x) => x.IsPublished ? GetStaticDate() : null)
					.RuleFor(x => x.ModifiedOn, GetStaticDate())
					.RuleFor(x => x.Author, _ => _userGenerator?.Generate());

	private static readonly Faker<AppUserModel> _userGenerator = new Faker<AppUserModel>()
			.UseSeed(421)
			.RuleFor(x => x.Id, _ => ObjectId.GenerateNewId())
			.RuleFor(x => x.UserName, f => f.Internet.UserName())
			.RuleFor(x => x.Email, f => f.Internet.Email());

	private static string GetSlug(this string item)
	{

		var slug = item.ToLower().Replace(" ", "-");

		// UrlEncode the slug
		slug = HttpUtility.UrlEncode(slug);

		return slug;

	}

	public static Uri ToUrl(this string slug, DateTimeOffset date)
	{

		return new Uri($"/{date.UtcDateTime:yyyyMMdd}/{slug}", UriKind.Relative);

	}

	private static DateTime GetStaticDate()
	{

		return new DateTime(2025, 1, 1);

	}

}
