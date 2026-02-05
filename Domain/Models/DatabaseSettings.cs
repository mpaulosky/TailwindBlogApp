// ============================================
// Copyright (c) 2023. All rights reserved.
// File Name :     DatabaseSettings.cs
// Company :       mpaulosky
// Author :        Matthew Paulosky
// Solution Name : TailwindBlog
// Project Name :  Domain
// =============================================

namespace Domain.Models;

/// <summary>
/// Database settings configuration.
/// </summary>
public record DatabaseSettings : IDatabaseSettings
{
	/// <summary>
	/// Initializes a new instance of the <see cref="DatabaseSettings"/> class.
	/// </summary>
	public DatabaseSettings() { }

	/// <summary>
	/// Initializes a new instance of the <see cref="DatabaseSettings"/> class with the specified settings.
	/// </summary>
	/// <param name="connectionStrings">The database connection string.</param>
	/// <param name="databaseName">The name of the database.</param>
	public DatabaseSettings(string connectionStrings, string databaseName)
	{
		ConnectionStrings = connectionStrings;
		DatabaseName = databaseName;
	}

	/// <summary>
	/// Gets or sets the database connection string.
	/// </summary>
	public string ConnectionStrings { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the database name.
	/// </summary>
	public string DatabaseName { get; set; } = string.Empty;
}