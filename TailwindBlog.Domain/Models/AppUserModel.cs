// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     AppUserModel.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain
// =======================================================

using MongoDB.Bson;

namespace TailwindBlog.Domain.Models;

public class AppUserModel
{

	public ObjectId Id { get; set; } = ObjectId.Empty;

	public string UserName { get; set; } = string.Empty;

	public string Email { get; set; } = string.Empty;

	public List<string> Roles { get; set; } = [];

public static AppUserModel Empty =>
		new()
		{
				Id = ObjectId.Empty,
				UserName = string.Empty,
				Email = string.Empty,
				Roles = []
		};

}
