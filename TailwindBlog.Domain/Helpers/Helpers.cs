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

namespace TailwindBlog.Domain.Helpers;

public static class Helpers
{
	private static readonly DateTime StaticDate = new(2025, 1, 1);

	private static DateTime GetStaticDate() => StaticDate;

	private static string GetSlug(this string item)
	{
		if (string.IsNullOrEmpty(item))
		{
			return string.Empty;
		}

		var slug = item.ToLower().Replace(" ", "-");
		return HttpUtility.UrlEncode(slug) ?? string.Empty;
	}

	private static readonly Faker<AppUserModel> _userGenerator = new Faker<AppUserModel>()
		.UseSeed(421)
		.RuleFor(x => x.Id, f => ObjectId.GenerateNewId().ToString())
		.RuleFor(x => x.UserName, f => f.Internet.UserName() ?? "user")
		.RuleFor(x => x.Email, f => f.Internet.Email() ?? "user@example.com")
		.RuleFor(x => x.Roles, _ => [])
		.FinishWith((f, u) =>
		{
			u.Id ??= ObjectId.GenerateNewId().ToString();
			u.UserName ??= "user";
			u.Email ??= "user@example.com";
			u.Roles ??= [];
		});

	public static readonly Faker<Article> ArticleGenerator =
		new Faker<Article>()
			.UseSeed(421)
			.CustomInstantiator(f =>
			{
				var title = f.WaffleTitle() ?? string.Empty;
				return new Article(
					title: title,
					introduction: f.WaffleTitle() ?? string.Empty,
					coverImageUrl: f.Image.PicsumUrl() ?? string.Empty,
					urlSlug: title.GetSlug(),
					author: _userGenerator.Generate() ?? AppUserModel.Empty,
					isPublished: f.Random.Bool(),
					publishedOn: f.Random.Bool() ? GetStaticDate() : null
				);
			})
			.RuleFor(x => x.Id, _ => Guid.NewGuid())
			.RuleFor(x => x.CreatedOn, _ => GetStaticDate())
			.RuleFor(x => x.ModifiedOn, _ => GetStaticDate());

	public static Uri ToUrl(this string slug, DateTimeOffset date)
	{
		return new Uri($"/{date.UtcDateTime:yyyyMMdd}/{slug}", UriKind.Relative);
	}
}
