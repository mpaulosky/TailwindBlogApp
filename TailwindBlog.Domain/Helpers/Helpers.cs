// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Helpers.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain
// =======================================================

namespace TailwindBlog.Domain.Helpers;

public static partial class Helpers
{

	private static readonly DateTime _staticDate = new(2025, 1, 1);

	public static DateTime GetStaticDate()
	{
		return _staticDate;
	}

	public static string GetSlug(this string item)
	{
		if (string.IsNullOrEmpty(item))
		{
			return string.Empty;
		}

		var slug = MyRegex().Replace(item.ToLower(), "_")
				.Trim('_');

		return HttpUtility.UrlEncode(slug);
	}

	public static Uri ToUrl(this string slug, DateTimeOffset date)
	{
		return new Uri($"/{date.UtcDateTime:yyyyMMdd}/{slug}", UriKind.Relative);
	}

	[System.Text.RegularExpressions.GeneratedRegex(@"[^a-z0-9]+")]
	private static partial System.Text.RegularExpressions.Regex MyRegex();

}