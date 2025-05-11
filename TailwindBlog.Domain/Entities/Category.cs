// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     Category.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.ApiService
// =======================================================

namespace TailwindBlog.Domain.Entities;

public class Category : Entity
{

	[Required(ErrorMessage = "Name is required"),
	MaxLength(80)]
	public string Name { get; set; } = string.Empty;

	[Required(ErrorMessage = "Description is required"),
	MaxLength(100)]
	public string Description { get; set; } = string.Empty;

	public static Category Empty =>
			new()
			{
					Id = ObjectId.Empty,
					Name = string.Empty,
					Description = string.Empty,
			};

}
