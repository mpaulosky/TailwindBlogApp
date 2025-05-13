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

namespace TailwindBlog.Domain.Helpers;

public static class Helpers
{

	public static readonly Faker<Article> ArticleGenerator =
		new Faker<Article>()
			.UseSeed(421)
			.CustomInstantiator(f =>
				new Article(
					title: f.WaffleTitle(),
					introduction: f.WaffleTitle(),
					coverImageUrl: f.Image.PicsumUrl(),
					urlSlug: f.WaffleTitle().GetSlug(),
					author: _userGenerator.Generate(),
					isPublished: f.Random.Bool(),
					publishedOn: f.Random.Bool() ? GetStaticDate() : null
				)
			)
			.RuleFor(x => x.Id, _ => ObjectId.GenerateNewId())
			.RuleFor(x => x.CreatedOn, _ => GetStaticDate())
			.RuleFor(x => x.ModifiedOn, _ => GetStaticDate());

	private static readonly Faker<AppUserModel> _userGenerator = new Faker<AppUserModel>()
			.UseSeed(421)
			.RuleFor(x => x.Id, f => ObjectId.GenerateNewId().ToString())
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
