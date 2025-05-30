// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     BlogDatabaseSettings.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  TailwindBlog.Domain
// =======================================================

namespace TailwindBlog.Domain.Models;

public class BlogDatabaseSettings
{

	public string ConnectionString { get; set; } = string.Empty;

	public string DatabaseName { get; set; } = string.Empty;

}