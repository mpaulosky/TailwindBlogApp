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
	public Category(string name, string description)
	{
		Name = name;
		Description = description;
	}

	// Parameterless constructor for EF Core and serialization
	private Category() { }

	[Required(ErrorMessage = "Name is required")]
	[MaxLength(80)]
	public string Name { get; private set; } = string.Empty;

	[Required(ErrorMessage = "Description is required")]
	[MaxLength(100)]
	public string Description { get; private set; } = string.Empty;

	public static Category Empty =>
		new(string.Empty, string.Empty)
		{
			Id = ObjectId.Empty
		};

}
