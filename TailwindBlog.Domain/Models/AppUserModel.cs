// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     AppUserModel.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain
// =======================================================

namespace TailwindBlog.Domain.Models;

public class AppUserModel
{

	public string Id { get; set; } = string.Empty;

	public string UserName { get; set; } = string.Empty;

	public string Email { get; set; } = string.Empty;

	public List<string> Roles { get; set; } = [];

	public static AppUserModel Empty =>
			new()
			{
					Id = string.Empty,
					UserName = string.Empty,
					Email = string.Empty,
					Roles = []
			};

}
