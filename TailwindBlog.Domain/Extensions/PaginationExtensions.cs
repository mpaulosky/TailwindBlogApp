// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     PaginationExtensions.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain
// =======================================================

namespace TailwindBlog.Domain.Extensions;

public static class PaginationExtensions
{
	public static IQueryable<T> Paginate<T>(this IQueryable<T> source, int pageNumber, int pageSize)
	{
		return source
				.Skip((pageNumber - 1) * pageSize)
				.Take(pageSize);
	}
}
