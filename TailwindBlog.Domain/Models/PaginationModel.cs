// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     PaginationModel.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain
// =======================================================

namespace TailwindBlog.Domain.Models;

public record PaginationModel<T>
{

	private const int MaxPageSize = 50;

	private readonly int _pageSize = 10;

	public int PageNumber { get; init; } = 1;

	public int PageSize {
		get => _pageSize;

		init => _pageSize = value > MaxPageSize ? MaxPageSize : value;
	}

	public int TotalPages { get; init; }

	public int TotalCount { get; init; }

	public bool HasPreviousPage => PageNumber > 1;

	public bool HasNextPage => PageNumber < TotalPages;

	public IReadOnlyCollection<T> Items { get; init; } = Array.Empty<T>();

	public static PaginationModel<T> Create(IEnumerable<T> items, int count, int pageNumber, int pageSize)
	{
		return new PaginationModel<T>
		{
				PageNumber = pageNumber,
				PageSize = pageSize,
				TotalCount = count,
				TotalPages = (int)Math.Ceiling(count / (double)pageSize),
				Items = items.ToArray()
		};
	}

}