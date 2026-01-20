// =======================================================
// Copyright (c) 2025. All rights reserved.
// File Name :     BlogDatabaseSettings.cs
// Company :       mpaulosky
// Author :        Matthew
// Solution Name : TailwindBlog
// Project Name :  Domain
// =======================================================

namespace Domain.Models;

/// <summary>
/// Configuration settings for the blog database.
/// </summary>
public record BlogDatabaseSettings
{
	/// <summary>
	/// Gets or sets the database connection string.
	/// </summary>
	public string ConnectionString { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the database name.
	/// </summary>
	public string DatabaseName { get; set; } = string.Empty;
}