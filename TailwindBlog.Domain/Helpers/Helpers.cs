// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Helpers.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain
// =======================================================

namespace TailwindBlog.Domain.Helpers;

public static class Helpers
{

	private static readonly DateTime _staticDate = new(2025, 1, 1);

	public static readonly Faker<Article> ArticleGenerator =
			new Faker<Article>()
					.UseSeed(421)
					.CustomInstantiator(f =>
					{
						var title = f.WaffleTitle();

						return new Article(
								title: title,
								introduction: f.WaffleTitle(),
								coverImageUrl: f.Image.PicsumUrl() ?? string.Empty,
								urlSlug: title.GetSlug(),
								author: UserGenerator.Generate() ?? AppUserModel.Empty,
								isPublished: f.Random.Bool(),
								publishedOn: f.Random.Bool() ? GetStaticDate() : null
						);
					})
					.RuleFor(x => x.Id, _ => ObjectId.GenerateNewId())
					.RuleFor(x => x.CreatedOn, _ => GetStaticDate())
					.RuleFor(x => x.ModifiedOn, _ => GetStaticDate());

	private static DateTime GetStaticDate() => _staticDate;

	private static string GetSlug(this string item)
	{
		if (string.IsNullOrEmpty(item))
		{
			return string.Empty;
		}

		var slug = item.ToLower().Replace(" ", "-");

		return HttpUtility.UrlEncode(slug);
	}

	public static readonly Faker<AppUserModel> UserGenerator =
			new Faker<AppUserModel>()
					.UseSeed(421)
					.RuleFor(x => x.Id, _ => ObjectId.GenerateNewId().ToString())
					.RuleFor(x => x.UserName, f => f.Internet.UserName() ?? "user")
					.RuleFor(x => x.Email, f => f.Internet.Email() ?? "user@example.com")
					.RuleFor(x => x.Roles, _ => []);

	public static Uri ToUrl(this string slug, DateTimeOffset date)
	{
		return new Uri($"/{date.UtcDateTime:yyyyMMdd}/{slug}", UriKind.Relative);
	}

}